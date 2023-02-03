function toggleQueues() {

    const joinedQueues = document.getElementById(
        'joined-queue-container',
    ) as HTMLInputElement | null;

    const createdQueues = document.getElementById(
        'created-queue-container',
    ) as HTMLInputElement | null;

    const joinedButton = document.querySelector(
        '.joined-button'
    ) as HTMLElement | null;

    const createdButton = document.querySelector(
        '.created-button'
    ) as HTMLElement | null;

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