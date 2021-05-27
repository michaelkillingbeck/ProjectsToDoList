function addNewTaskToList(taskListContainerId, taskName) {
    let taskListContainer = document.getElementById(taskListContainerId);

    let newTaskElement = document.createElement("li");
    let newInputElement = document.createElement("input");
    newInputElement.classList.add("notepad-line-item");
    newInputElement.classList.add("new-task");
    newInputElement.classList.add("new-project-placeholder");
    newInputElement.value = taskName;
    newInputElement.name = "NewProject.ProjectTasks";
    newTaskElement.appendChild(newInputElement);
    taskListContainer.prepend(newTaskElement);
}

function hideOverflowedTasks(pageSizeElementId) {
    let pageSizeElement = document.getElementById(pageSizeElementId);
    let pageSize = Number(pageSizeElement.value);
    let allTasks = document.querySelectorAll("#project-tasks > li");

    allTasks.forEach((task, index) => {
        if(index >= pageSize) {
            task.style.display = 'none';
        }
    });
}

export { addNewTaskToList, hideOverflowedTasks };