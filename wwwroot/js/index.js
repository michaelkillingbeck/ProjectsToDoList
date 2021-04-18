document.getElementById("create-new-project").addEventListener("focus", function() {
    this.innerHTML = '';
});

document.getElementById("create-new-project").addEventListener("blur", function() {
    this.innerHTML = 'Create new...';
});

document.getElementById("create-new-project").addEventListener("keypress", function(event) {
    if(event.key === "Enter") {
        event.preventDefault();
        let projectName = this.textContent;

        $.ajax({
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: {
                ProjectName: projectName,
            },
            method: "POST",
            url: 'Index?handler=SaveNewProject',
        })
        .always(function () {
            window.location.reload();   
        });

        return false;
    }
});