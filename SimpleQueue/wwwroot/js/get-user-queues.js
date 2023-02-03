function toggleQueues() {
    var joinedQueues = document.getElementById('joined-queue-container');
    var createdQueues = document.getElementById('created-queue-container');
    var joinedButton = document.querySelector('.joined-button');
    var createdButton = document.querySelector('.created-button');
    console.log(joinedQueues);
    if (joinedQueues.style.display == 'block') {
        joinedQueues.style.display = 'none';
        createdQueues.style.display = 'block';
        joinedButton.style.height = '68px';
        joinedButton.style.marginTop = '52px';
        createdButton.style.height = '120px';
        createdButton.style.marginTop = '0';
    }
    else {
        joinedQueues.style.display = 'block';
        createdQueues.style.display = 'none';
        joinedButton.style.height = '120px';
        joinedButton.style.marginTop = '0';
        createdButton.style.height = '68px';
        createdButton.style.marginTop = '52px';
    }
}
//# sourceMappingURL=get-user-queues.js.map