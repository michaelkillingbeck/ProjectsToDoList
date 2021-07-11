document.getElementById("current-project-private").addEventListener("input", function (event) {
    let privateField = document.getElementById("CurrentProject_Private");

    privateField.value = this.checked;
});