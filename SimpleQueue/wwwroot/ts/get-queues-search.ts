const myHeaders = new Headers();
console.log(myHeaders.get("pagination"));

class QueueParameters {
    StartTime: string = "";
    EndTime: string = "";
    SearchTerm: string = "";
    IsFrozen: boolean = null;
    IsChat: boolean = null;
    IsProtected: boolean = null;
}

let queueParams: QueueParameters = new QueueParameters();
setQueueParam();

function setQueueParam() {
    const searchValue = document.getElementById(
        'search-value',
    ) as HTMLInputElement | null;
    queueParams.SearchTerm = searchValue.value;

    const startTimeValue = document.getElementById(
        'start-time-value',
    ) as HTMLInputElement | null;
    queueParams.StartTime = startTimeValue.value;

    const endTimeValue = document.getElementById(
        'end-time-value',
    ) as HTMLInputElement | null;
    queueParams.EndTime = endTimeValue.value;
}

function search() {
    const searchValue = document.getElementById(
        'search-value',
    ) as HTMLInputElement | null;

    queueParams.SearchTerm = searchValue.value;

    console.log(queueParams.SearchTerm);
}

function startTime() {
    const startTimeValue = document.getElementById(
        'start-time-value',
    ) as HTMLInputElement | null;

    queueParams.StartTime = startTimeValue.value;

    console.log(queueParams.StartTime);
}

function endTime() {
    const endTimeValue = document.getElementById(
        'end-time-value',
    ) as HTMLInputElement | null;

    queueParams.EndTime = endTimeValue.value;

    console.log(queueParams.EndTime);
}

function freeze() {
    const freezeValue = document.getElementById(
        'freeze-value',
    ) as HTMLInputElement | null;

    queueParams.IsFrozen = getBooleanValue(freezeValue.value);

    console.log(queueParams.IsFrozen);
}

function chat() {
    const chatValue = document.getElementById(
        'chat-value',
    ) as HTMLInputElement | null;

    queueParams.IsChat = getBooleanValue(chatValue.value);

    console.log(queueParams.IsChat);
}

function privacy() {
    const privacyValue = document.getElementById(
        'privacy-value',
    ) as HTMLInputElement | null;

    queueParams.IsProtected = getBooleanValue(privacyValue.value);

    console.log(queueParams.IsProtected);
}

function getBooleanValue(value: string) {
    if (value == "") {
        return null;
    }

    return Boolean(Number(value))
}

const urlQueues = "https://localhost:7253/queues";

function findQueues() {
    console.log(queueParams);
    const url = urlQueues +
        `?StartTime=${queueParams.StartTime}&EndTime=${queueParams.EndTime}&SearchTerm=${queueParams.SearchTerm}&IsFrozen=${queueParams.IsFrozen}&IsChat=${queueParams.IsChat}&IsProtected=${queueParams.IsProtected}`;
    window.location.href = url;
}