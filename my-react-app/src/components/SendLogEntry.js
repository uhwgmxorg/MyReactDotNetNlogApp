import Service from "../services/Service";
import './SendLogEntry.css';

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
            <div class="app-field-container">
                <label htmlFor="filter">Filter:</label>
                <input type="Number" id="filter" placeholder="100" />
                <label htmlFor="user">User:</label>
                <input type="text" id="user" placeholder="User01" />
                <label htmlFor="message">Message:</label>
                <input type="text" id="message" placeholder="Message Hello :-)" />
                <button onClick={onButton_1}>
                    Submit to NLog
                </button>
            </div>
        </div>
    );
}

export default SendLogEntry;