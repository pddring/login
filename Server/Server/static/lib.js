const eHints = document.getElementById("hints");
const eForm = document.getElementById("login");

function reset() {
    
}

function createUser() {
    
}

function login() {
    let username = document.getElementById("username").value
    let password = document.getElementById("password").value
    try {
        presenceCheck(username, "Please enter your username");
        presenceCheck(password, "Please enter your password");
        lengthCheck(password, 5, 10, "Please enter a password between 5 and 10 characters");
        eHints.innerText = "Attempting login";
        eForm.submit();
    } catch (e) {
        eHints.innerText = e;
    }
}

function lengthCheck(input, minLength, maxLength, message) {
    if (input.length > maxLength || input.length < minLength) {
        throw message;
    }
}

function presenceCheck(input, message) {
    if (input == "") {
        throw message;
    } 
}