$(function () {
    let theCompiledTemplete = Handlebars.compile($('#ProjectTemplate').html());
    $.get("/ToDo/ShowAll", function (data, status, xhr) {
        for (let i = 0; i < data.length; i++) {
            $("#AddList").before(theCompiledTemplete(data[i]));
            for (let j = 0; j < data[i].JobTasks.length; j++) {
                createTask(data[i].JobTasks[j]);
            }
            $('[data-todo="' + data[i].ListId + '"]').mixItUp({ layout: { display: 'block' } });

            let list = data[i].ListId;

            $('[data-list=' + data[i].ListId + ']').find(".AddTaskName").keydown(function (eventObject) {
                if (event.keyCode == 13) {
                    displayTask(list);
                }
            });
            $('[data-list=' + data[i].ListId + ']').find(".doubleclicklist").dblclick(function () {
                updateList(list);
            });
        }
    });
});



function addProject() {
    $.ajax({
        url: '/ToDo/Add',
        type: 'POST',
        success: function (data) {
            createList(data);
            $('[data-todo=' + data.ListId + ']').mixItUp({ layout: { display: 'block' } });
        },
        error: function () {
            alert("Error");
        }
    });
}

function createList(data) {
    let t = Handlebars.compile($('#ProjectTemplate').html());
    $("#AddList").before(t(data));
    $('[data-list=' + data.ListId + ']').css("display", "none");
    $('[data-list=' + data.ListId + ']').slideDown();
    $('[data-list=' + data.ListId + ']').find(".doubleclicklist").dblclick(function () {
        updateList(data.ListId);
    });
    $('[data-list=' + data.ListId + ']').find(".ListName").dblclick(function () {
        updateList(data.ListId);
    });
    $('[data-list=' + data.ListId + ']').find(".AddTaskName").keydown(function (eventObject) {
        if (event.keyCode == 13) {
            displayTask(data.ListId);
        }
    });
}

function deleteList(id) {
    $.ajax({
        url: '/ToDo/Delete',
        data: { id: id },
        type: 'POST',
        success: function (data) {
            $('[data-todo=' + data.id + ']').slideUp(1000, function () { this.remove(); })
        },
        error: function () {
            alert("Error");
        }
    });
}

function updateList(id) {
    $('[data-list=' + id + ']').find('.ListName').prop("disabled", false);

    let listName = $('[data-list=' + id + ']').find('.ListName');
    let currentName = listName.val();

    listName.prop("disabled", false).focus();
    listName.prop({
        'selectionStart': currentName.length,
    });

    listName.keydown(function (eventObject) {
        if (event.keyCode == 13) {

            let newName = listName.val();

            if (newName == "") {
                alert("empty!");
                event.preventDefault();
            } else {

                $('[data-list=' + id + ']').find('.ListName').prop("disabled", true);

                $.ajax({
                    url: '/List/Update',
                    data: { id: id, name: newName },
                    type: 'POST',
                    error: function () {
                        alert("Error");
                    }
                });
            }
        }
    });

}
function displayTask(id) {
    let TaskName = $('[data-list=' + id + ']').find('.AddTaskName').val();

    if (TaskName == "") {
        alert("empty!");
        event.preventDefault();
    }

    else {
        $.ajax({
            url: '/JobTask/Add',
            data: {
                id: id,
                name: TaskName
            },
            type: 'POST',
            success: function (task) {
                createTask(task);
                $('[data-list=' + id + ']').find('.AddTaskName').val("");
            },
            error: function () {
                alert("Error");
            }
        });
    }
}
function createTask(data) {

    let t = Handlebars.compile($('#TaskTemplate').html());

    $('[data-todo=' + data.ListId + ']').append(t(data));
    $('[data-picker=' + data.TaskId + ']').datepicker({
        autoclose: true,
        clearBtn: true,
        pickTime: false,
        format: "mm/dd/yyyy"
    });
    $('[data-picker=' + data.TaskId + ']').datepicker().on('changeDate', function (e) {
        let deadline = $('[data-picker=' + data.TaskId + ']').val()
        setDate(data.TaskId, deadline);
    });
    if (data.DeadLine == null) {
        $('[data-picker=' + data.TaskId + ']').val("");
    } else {
        let date = new Date(parseInt(data.DeadLine.substr(6)));
        let Year = date.getFullYear();
        let Month = date.getMonth() + 1;
        Month = Month > 9 ? Month : "0" + Month;
        let Day = date.getDate();
        Day = Day > 9 ? Day : "0" + Day;
        $('[data-picker=' + data.TaskId + ']').val(Month + "/" + Day + "/" + Year);
    }
    if (data.Done == true) {
        $('[data-check=' + data.TaskId + ']').prop('checked', 'true');
        $('[data-task=' + data.TaskId + ']').find('.TaskName').css("text-decoration", "line-through");
    }

    $('[data-task=' + data.TaskId + ']').css("display", "none");
    $('[data-task=' + data.TaskId + ']').slideDown();

    $('[data-task=' + data.TaskId + ']').find(".doubleclick").dblclick(function () {
        updateTask(data.TaskId);
    });
}
function deleteTask(id) {
    $.ajax({
        url: '/JobTask/Delete',
        data: {
            id: id
        },
        type: 'POST',

        success: function (data) {
            $('[data-task=' + id + ']').animate({ "left": "+10000px" }, "slow").slideUp(800, function () { this.remove(); });
        },
        error: function () {
            alert("Error");
        }
    });
}

function updateTask(id) {
    let taskName = $('[data-task=' + id + ']').find('.TaskName');
    let currentName = taskName.val();

    taskName.prop("disabled", false).focus();

    taskName.prop({
        'selectionStart': currentName.length,
    });

    taskName.keydown(function (eventObject) {
        if (event.keyCode == 13) {

            let newName = taskName.val();
            if (newName == "") {
                alert("empty!");
                event.preventDefault();
            }
            else {
                taskName.prop("disabled", true);
                $.ajax({
                    url: '/JobTask/Update',
                    type: 'POST',
                    data: {
                        id: id,
                        name: newName
                    },
                    error: function () {
                        alert("Error");
                    }
                });
            }
        }
    });
}
function isChecked(id, checkbox) {

    let taskName = $('[data-task=' + id + ']').find('.TaskName');
    let done = checkbox.checked;

    if (done == true) {
        taskName.css("text-decoration", "line-through");
    }
    else {
        taskName.css("text-decoration", "none");
    }
    $.ajax({
        url: '/JobTask/Check',
        method: 'POST',
        data: {
            "id": id,
            "done": done
        },
        error: function () {
            alert("Error");
        }
    });

}
function setDate(id, date) {
    $.ajax({
        url: '/JobTask/SetDate',
        data: {
            "id": id,
            "date": date
        },
        method: 'POST',
        error: function () {
            alert("Error");
        }
    });
}

function priority(listId, taskId, type) {
    let currentPriority = $('[data-task=' + taskId + ']').attr('data-priority');
    $.ajax({
        url: '/JobTask/Priority',
        type: 'POST',
        data: {
            ListId: listId,
            CurrentPriority: currentPriority,
            CurrentId: taskId,
            Type: type
        },
        type: 'POST',
        success: function (data) {
            $('[data-task=' + data.CurrentId + ']').attr('data-priority', data.TargetPriority);
            $('[data-task=' + data.TargetId + ']').attr('data-priority', data.CurrentPriority);
            $('[data-todo=' + data.ListId + ']').mixItUp('sort', 'priority');
        },
        error: function () {
            alert("Error");
        }
    });
}

