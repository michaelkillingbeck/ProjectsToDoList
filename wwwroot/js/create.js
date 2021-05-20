import { addTask, addNewTaskToList } from './createNewProject.js';

document.getElementById("new-project-task").addEventListener("keypress", function(event) {
    if(event.key === "Enter") {
        event.preventDefault();
        let taskName = this.value;

        addTask(taskName);
        addNewTaskToList('new-task', 'notepad-page');
        this.value = null;

        return false;
    }
});