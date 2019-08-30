import React, {Component} from 'react';
import CleaningTaskElement from "./CleaningTaskElement";
import WindowTaskElement from "./WindowTaskElement";
import HelperMethods from "./HelperMethods";
import DocumentLinkButton from "./DocumentLinkButton";
import LoadingIndicator from "./LoadingIndicator";

let Config = require('./config.json');


class CleaningTask extends Component {
    constructor(props) {
        super(props);
        if (props.location.state !== undefined && props.location.state.floors === undefined) {
            this.state = {
                tasks: props.location.state.tasks,
                locationID: props.match.params.locationID,
                planType: props.match.params.type,
                floorID: props.match.params.floor,
            };
        } else {
            this.state = {
                tasks: [],
                locationID: props.match.params.locationID,
                planType: props.match.params.type,
                floorID: props.match.params.floor,
            };
            this.getTasks();
        }
    }

    getTasks() {
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        let fetchUrl = this.state.planType !== "1"
            ? "CleaningTask/TasksFromPlan/" + this.state.locationID + "/" + this.state.planType + "/"
            : "CleaningTask/TasksFromFloor/" + this.state.locationID + "/" + this.state.planType + "/" + this.state.floorID;
        HelperMethods.getRequest(fetchUrl, myHeaders,
            (tasks) => {
                this.setState({tasks: tasks});
            }, undefined, () => {
            });
    }

    render() {
        //Sample url: /Customers/1/Locations/6/CleaningPlans/2/Vinduespudsning/
        let urlArray = this.props.match.url.split("/");
        let customerIDPostion = urlArray.findIndex((s) => s === "Customers") + 1; //Find the index of customers in the URL string and add 1 to get the index of the the customer position.
        let customerID = urlArray[customerIDPostion];
        return (
            <div className="col-10" style={{margin: 'auto'}}>
                {this.state.planType !== "1" &&
                <DocumentLinkButton locationID={this.state.locationID} planType={this.state.planType}
                                    customerID={customerID}/>
                }
                {this.state.tasks.length ?
                    this.state.tasks.map(taskArea =>
                        <div key={taskArea.area.id}>
                            {taskArea.tasks[0].planType === 0 ?
                                <div className="reg-cp-title-content">
                                    <div className="reg-cp-area">
                                        <div className="reg-cp-area-text">
                                            <svg xmlns="http://www.w3.org/2000/svg"
                                                 xmlnsXlink="http://www.w3.org/1999/xlink" version="1.1"
                                                 width={24}
                                                 height={24} viewBox="0 0 24 24">
                                                <path
                                                    d="M17.63,5.84C17.27,5.33 16.67,5 16,5H5A2,2 0 0,0 3,7V17A2,2 0 0,0 5,19H16C16.67,19 17.27,18.66 17.63,18.15L22,12L17.63,5.84Z"/>
                                            </svg>
                                            <h2>{taskArea.area.description}</h2>
                                        </div>
                                    </div>
                                </div>
                                :
                                <div/>
                            }
                            {taskArea.tasks.map(task =>
                                <div>
                                    {task.frequency !== null
                                        ? <CleaningTaskElement task={task}/>
                                        : <WindowTaskElement task={task} match={this.props.match}
                                                             pathname={this.props.location.pathname}/>
                                    }
                                </div>
                            )}
                        </div>
                    )
                    :
                    <LoadingIndicator/>}
            </div>
        );
    }
}

export default CleaningTask;