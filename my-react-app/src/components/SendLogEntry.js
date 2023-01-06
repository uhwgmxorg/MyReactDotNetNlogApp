import Service from "../services/Service";

function onButton_1() {
    const filter = document.getElementById('filter').valueAsNumber;
    const user = document.getElementById('user').value;
    const message = document.getElementById('message').value;
    //alert("Send: " + filter + " " + user + " " + message + " to NLog in DB");
    Service.postData(filter, user, message)
        .catch((e) => {
            console.log(e);
        });
}

const SendLogEntry = () => {
    return (
        <div>
            <h2>React-Client of MyDotNetNlogApi</h2>
            <p>A React application to demonstrate NLog in the backlog driven by a React application.</p>
            <div>
                <input type="Number" id="filter"/>
                <input type="text" id="user" />
                <input type="text" id="message" />
                <button onClick={onButton_1}>
                    Submit to NLog
                </button>
            </div>
        </div>
    );
}

export default SendLogEntry;