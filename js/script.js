function validateForm() {
    console.log("Form validation function called!");
}

//loaded
console.log("External script.js loaded successfully!");





/*FORM VALIDATION*/
function validateSignUp() {
    
    //Get all the inputs
    let username = document.getElementById('signupUsername').value;
    let email = document.getElementById('signupEmail').value;
    let pass1 = document.getElementById('signupPassword').value;
    let pass2 = document.getElementById('confirmPassword').value;
    
    //Get the error message
    let errorMessage = document.getElementById('error-message');
    
    //if else to check
    if (username === "" || email === "" || pass1 === "" || pass2 === "") {
        errorMessage.textContent = "Error: All fields must be filled out.";
        return false;
    }
    
    if (pass1 !== pass2) {
        errorMessage.textContent = "Error: Passwords do not match.";
        return false;
    }

    errorMessage.textContent = "";
    console.log("Form is valid!");
    alert("Sign up successful! (This is a simulation)");
    return false;
}




/*DASHBOARD*/
function selectChallenge(buttonElement) {
    //Get the text from the button's parent
    let challengeCard = buttonElement.parentElement;
    let challengeTitle = challengeCard.querySelector('h3').textContent;

    //Get the elements we want to change
    let statusText = document.getElementById('challenge-status-text');
    let statusImage = document.getElementById('challenge-status-image');

    //Change the elements
    statusText.textContent = "You have started the challenge: " + challengeTitle;
    
    
    // We'll use a placeholder image URL
    statusImage.src = "https://i.imgur.com/kP4wB3A.png"; // Using the same logo as placeholder
    
    //Change CSS Styles
    statusImage.style.display = "block";

    //Change CSS style of the button itself
    buttonElement.textContent = "In Progress...";
    buttonElement.style.backgroundColor = "#5cb85c";
    buttonElement.disabled = true;
}