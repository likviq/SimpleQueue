const myHeaders = new Headers();
console.log(myHeaders.get("pagination"));

let pageNumber = 1;
let pageSize = 10;

class QueueParameters {
    StartTime: string = "";
    EndTime: string = "";
    SearchTerm: string = "";
    IsFrozen: boolean = null;
    IsChat: boolean = null;
    IsProtected: boolean = null;
    PageNumber: number = pageNumber;
    PageSize: number = pageSize;
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
        `?StartTime=
${queueParams.StartTime}&EndTime=${queueParams.EndTime}&SearchTerm=
${queueParams.SearchTerm}&IsFrozen=${queueParams.IsFrozen}&IsChat=
${queueParams.IsChat}&IsProtected=${queueParams.IsProtected}`;
    window.location.href = url;
}

interface Queue {
    id: string;
    title: string;
    description: string;
    isFrozen: string;
}

async function uploadMoreQueues() {
    let queues = await findMoreQueues<Queue[]>();
    console.log(queues);

    console.log(document.querySelector(".queue-body"))

    for (let i = 0; i < queues.length; i++) {
        let elem = document.querySelector('.queue-body:last-of-type');

        let clone = elem.cloneNode(true);

        elem.setAttribute('href', '/queue/' + queues[i].id);

        elem.getElementsByClassName('queue-title')[0]
            .innerHTML = queues[i].title;

        elem.getElementsByClassName('queue-description')[0]
            .innerHTML = queues[i].description;

        elem.before(clone);
    }
}

function hideMoreButton() {
    const moreButton = document.getElementById(
        'more-button',
    ) as HTMLInputElement | null;

    moreButton.style.display = 'none';
}

function findMoreQueues<TResponse>(): Promise<TResponse> {
    queueParams.PageNumber += 1;
    let url = prepareUrl();
    const encodedUrl = encodeURI(url);

    return fetch(encodedUrl)
        .then(function (response) {
            if (response.ok) {
                hideMoreButton();
                console.log(response.headers.get("pagination"));
                return response.json()
            }
            throw new Error(response.statusText);
        })
        .then((data) => data as TResponse);

}

const urlApiQueues = "https://localhost:7147/api/queues";

function prepareUrl() {

    const startTimeQuery = queueParams.StartTime == "" ? "" : `&StartTime=${queueParams.StartTime}`;
    const endTimeQuery = queueParams.EndTime == "" ? "" : `&EndTime=${queueParams.EndTime}`;
    const searchTermQuery = queueParams.SearchTerm == "" ? "" : `&SearchTerm=${queueParams.SearchTerm}`;

    const isFrozenQuery = queueParams.IsFrozen == null ? "" : `&IsFrozen=${queueParams.IsFrozen}`;
    const isChatQuery = queueParams.IsChat == null ? "" : `&IsChat=${queueParams.IsChat}`;
    const isProtectedQuery = queueParams.IsProtected == null ? "" : `&IsProtected=${queueParams.IsProtected}`;

    return urlApiQueues +
        `?PageNumber=${queueParams.PageNumber}&PageSize=${queueParams.PageSize}`
        + startTimeQuery + endTimeQuery + searchTermQuery
        + isFrozenQuery + isChatQuery + isProtectedQuery;
}