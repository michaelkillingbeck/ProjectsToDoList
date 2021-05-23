import { addNewTaskToList, hideOverflowedTasks } from './createNewProject.js';

document.getElementById("new-project-task").addEventListener("keypress", function(event) {
    if(event.key === "Enter") {
        event.preventDefault();
        let taskName = this.value;

        addNewTaskToList('project-tasks', taskName);

        this.value = null;
        hideOverflowedTasks('PageSize');
        return false;
    }
});