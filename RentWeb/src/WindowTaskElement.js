import React, {Component} from 'react';
import {Link, Redirect} from "react-router-dom";

class WindowTaskElement extends Component {
    constructor(props) {
        super(props);
        this.state = {redirect: false};
    }

    render() {
        return (
            <Link to={`${this.props.pathname}History/${this.props.task.id}/`}>
                <div className="win-fan-card-content pointer">
                    <div className="win-fan-card">
                        <div className="win-fan-card-header">
                            <div className="win-fan-card-title">
                                <h2>{this.props.task.area.description}</h2>
                            </div>
                        </div>
                        <div className="win-fan-card-details d-flex justify-content-between">
                            <div className="win-fan-card-details-interval">
                                <svg xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink"
                                     version="1.1" width={24} height={24} viewBox="0 0 24 24">
                                    <path
                                        d="M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2M12,4A8,8 0 0,1 20,12A8,8 0 0,1 12,20A8,8 0 0,1 4,12A8,8 0 0,1 12,4M12,6A6,6 0 0,0 6,12A6,6 0 0,0 12,18A6,6 0 0,0 18,12A6,6 0 0,0 12,6M12,8A4,4 0 0,1 16,12A4,4 0 0,1 12,16A4,4 0 0,1 8,12A4,4 0 0,1 12,8M12,10A2,2 0 0,0 10,12A2,2 0 0,0 12,14A2,2 0 0,0 14,12A2,2 0 0,0 12,10Z"/>
                                </svg>
                                <p>{this.props.task.timesCleanedThisYear} / {this.props.task.timesThisYear}</p>
                            </div>
                            <div className="win-fan-card-details-last-clean-date">
                                <svg xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink"
                                     version="1.1" width={24} height={24} viewBox="0 0 24 24">
                                    <path
                                        d="M19.36,2.72L20.78,4.14L15.06,9.85C16.13,11.39 16.28,13.24 15.38,14.44L9.06,8.12C10.26,7.22 12.11,7.37 13.65,8.44L19.36,2.72M5.93,17.57C3.92,15.56 2.69,13.16 2.35,10.92L7.23,8.83L14.67,16.27L12.58,21.15C10.34,20.81 7.94,19.58 5.93,17.57Z"/>
                                </svg>
                                <p>{this.props.task.lastTaskCompleted !== null ? new Date(this.props.task.lastTaskCompleted.completedDate).toLocaleDateString() : "Ikke rengjort endnu"}</p>
                            </div>
                        </div>
                        <div className="win-fan-card-interval-text-wrapper">
                            <div className="win-fan-card-interval-text">
                                <p>{this.props.task.lastTaskCompleted !== null ? this.props.task.lastTaskCompleted.comment : ""}</p>
                            </div>
                        </div>
                    </div>
                </div>
            </Link>


        );
    }
}

export default WindowTaskElement;
//http://jsfiddle.net/weqm8q5w/6/