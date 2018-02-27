$(function () {


    var t = Handlebars.compile($('#ListTemplate').html());
    $.get("/List/Render", function (data, status, xhr) {
        for (var i = 0; i < data.length; i++) {
            $("#AddList").before(t(data[i]));
            for (var j = 0; j < data[i].Tasks.length; j++) {
                createTask(data[i].Tasks[j]);
            }
            $('[data-todo="' + data[i].ListId + '"]').mixItUp({ layout: { display: 'block' } });

            var list = data[i].ListId;

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

function displayData() {
    $.ajax({
        url: '/List/Add',
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
    var t = Handlebars.compile($('#ListTemplate').html());
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
        url: '/List/Delete',
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

    var listName = $('[data-list=' + id + ']').find('.ListName');
    var currentName = listName.val();

    listName.prop("disabled", false).focus();
    listName.prop({
        'selectionStart': currentName.length,
    });

    listName.keydown(function (eventObject) {
        if (event.keyCode == 13) {

            var newName = listName.val();

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
    var TaskName = $('[data-list=' + id + ']').find('.AddTaskName').val();

    if (TaskName == "") {
        alert("empty!");
        event.preventDefault();
    }

    else {
        $.ajax({
            url: '/Task/Add',
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

    var t = Handlebars.compile($('#TaskTemplate').html());

    $('[data-todo=' + data.ListId + ']').append(t(data));
    $('[data-picker=' + data.TaskId + ']').datepicker({
        autoclose: true,
        clearBtn: true,
        pickTime: false,
        format: "mm/dd/yyyy"
    });
    $('[data-picker=' + data.TaskId + ']').datepicker().on('changeDate', function (e) {
        var deadline = $('[data-picker=' + data.TaskId + ']').val()
        setDate(data.TaskId, deadline);
    });
    if (data.DeadLine == null) {
        $('[data-picker=' + data.TaskId + ']').val("");
    } else {
        var date = new Date(parseInt(data.DeadLine.substr(6)));
        var Year = date.getFullYear();
        var Month = date.getMonth() + 1;
        Month = Month > 9 ? Month : "0" + Month;
        var Day = date.getDate();
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
        url: '/Task/Delete',
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
    var taskName = $('[data-task=' + id + ']').find('.TaskName');
    var currentName = taskName.val();

    taskName.prop("disabled", false).focus();

    taskName.prop({
        'selectionStart': currentName.length,
    });

    taskName.keydown(function (eventObject) {
        if (event.keyCode == 13) {

            var newName = taskName.val();
            if (newName == "") {
                alert("empty!");
                event.preventDefault();
            }
            else {
                taskName.prop("disabled", true);
                $.ajax({
                    url: '/Task/Update',
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

    var taskName = $('[data-task=' + id + ']').find('.TaskName');
    var done = checkbox.checked;

    if (done == true) {
        taskName.css("text-decoration", "line-through");
    }
    else {
        taskName.css("text-decoration", "none");
    }
    $.ajax({
        url: '/Task/Check',
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
        url: '/Task/SetDate',
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
    var currentPriority = $('[data-task=' + taskId + ']').attr('data-priority');
    $.ajax({
        url: '/Task/Priority',
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

