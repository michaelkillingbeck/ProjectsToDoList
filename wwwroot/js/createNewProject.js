let newTaskClass = 'new-task';
let tasks = [];

function addTask(taskName) {
    tasks.push(taskName);
}

function addNewTaskToList(className, taskListContainerClassName) {
    let taskListContainer = document.getElementsByClassName(taskListContainerClassName)[0];
    let taskList = document.getElementsByClassName(className);
    let taskListArray = Object.values(taskList);

    tasks.forEach((task, index) => {
        if(index < taskListArray.length) {
            taskList[index].value = task;
            taskList[index].name = "NewProject.ProjectTasks";
        } else {
            let newTaskElement = document.createElement("li");
            newTaskElement.hidden = true;
            let newInputElement = document.createElement("input");
            newInputElement.value = tasks[index];
            newInputElement.name = "NewProject.ProjectTasks";
            newTaskElement.appendChild(newInputElement);
            taskListContainer.appendChild(newTaskElement);
        }
    });
}

export { addTask, addNewTaskToList };