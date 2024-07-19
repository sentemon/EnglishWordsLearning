﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// This function sets focus on the userTranslation input field
function setFocus() {
    document.getElementById("userTranslation").focus();
}

// Set focus when the page loads
window.onload = setFocus;

// Set focus after the form is submitted
document.getElementById("translationForm").onsubmit = function() {
    setTimeout(setFocus);
};

// switch the theme (light / dark)

// 
function showPassword(inputId) {
    let checkbox = document.getElementById(inputId);
    
    if (checkbox.type === "password") {
        checkbox.type = "text";
    } 
    else {
        checkbox.type = "password";
    }
}