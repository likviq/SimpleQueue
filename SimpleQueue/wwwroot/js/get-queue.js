var apiEndpointUri = "https://localhost:7253/api/callapi";
var isFrozen = false;
setFreezeVariable();
var amountOfParticipant = 0;
setAmountOfParticipant();
var clickerValue = 0;
function clicker() {
    clickerValue = clickerValue + 1;
    var clickerNumber = document.getElementById('clicker-value');
    clickerNumber.textContent = clickerValue.toString();
}
function nextUser(idQueue) {
    var uri = "https://localhost:7147/api/queue/".concat(idQueue, "/next");
    var method = "post";
    api(apiEndpointUri, method, uri);
    var firstUser = document.getElementsByClassName('user-position')[0];
    firstUser.remove();
    decreaseAmountOfParticipant();
}
function deleteUserFromQueue(idQueue, idParticipant) {
    var uri = "https://localhost:7147/api/queue/".concat(idQueue, "/participant/").concat(idParticipant);
    var method = "post";
    api(apiEndpointUri, method, uri);
    var userField = document.getElementById('user-' + idParticipant);
    userField.remove();
    decreaseAmountOfParticipant();
}
function decreaseAmountOfParticipant() {
    var participants = document.getElementById('admin-number');
    amountOfParticipant -= 1;
    participants.textContent = amountOfParticipant.toString();
}
function increaseAmountOfParticipant() {
    var participants = document.getElementById('admin-number');
    amountOfParticipant += 1;
    participants.textContent = amountOfParticipant.toString();
}
function freezeQueue(id) {
    var uri = "https://localhost:7147/api/queue/" + id;
    var method = "post";
    api(apiEndpointUri, method, uri);
    isFrozen = !isFrozen;
    var statusQueueImage = document.getElementById('status-queue-image');
    var freezeImage = document.getElementById('freeze-button');
    var unFreezeImage = document.getElementById('unfreeze-button');
    if (isFrozen == true) {
        freezeImage.style.display = 'none';
        unFreezeImage.style.display = '';
        statusQueueImage.style.backgroundColor = '';
    }
    else {
        freezeImage.style.display = '';
        unFreezeImage.style.display = 'none';
        statusQueueImage.style.backgroundColor = '#82FF9D';
    }
}
function setFreezeVariable() {
    var statusQueueImage = document.getElementById('status-queue-image');
    var imageColor = statusQueueImage.style.backgroundColor;
    if (imageColor == '') {
        isFrozen = true;
    }
}
function setAmountOfParticipant() {
    var participants = document.getElementById('admin-number');
    amountOfParticipant = parseInt(participants.textContent);
}
function api(url, method, uri) {
    url = prepareRequest(url, method, uri);
    return fetch(url)
        .then(function (response) {
        if (!response.ok) {
            throw new Error(response.statusText);
        }
        console.log(response.text);
        return response.json();
    })
        .then(function (data) {
        return data.data;
    });
}
function prepareRequest(url, method, uri) {
    return url + "?" + "method=" + method + "&uri=" + uri;
}
//# sourceMappingURL=get-queue.js.map