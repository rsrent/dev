import React, {Component} from 'react';

class CleaningTaskElement extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className="reg-cp-card-content" key={this.props.task.id}>
                <div className="reg-cp-card">
                    <div className="reg-cp-card-header">
                        <div className="reg-cp-card-title">
                            <svg xmlns="http://www.w3.org/2000/svg"
                                 xmlnsXlink="http://www.w3.org/1999/xlink"
                                 version="1.1" width={24} height={24}
                                 viewBox="0 0 24 24">
                                <path
                                    d="M17.63,5.84C17.27,5.33 16.67,5 16,5H5A2,2 0 0,0 3,7V17A2,2 0 0,0 5,19H16C16.67,19 17.27,18.66 17.63,18.15L22,12L17.63,5.84Z"/>
                            </svg>
                            <h2>{this.props.task.area.description}{this.props.task.comment !== null && this.props.task.comment !== "" && " - " + this.props.task.comment
                            }</h2>
                        </div>
                    </div>
                    <div className="reg-cp-card-details d-flex justify-content-between">
                        <div className="reg-cp-card-details-code">
                            <p>Frekvens: {this.props.task.frequency}</p>
                        </div>
                        {this.props.task.squareMeters !== 0 &&
                        <div className="reg-cp-card-details-size">
                            <svg xmlns="http://www.w3.org/2000/svg"
                                 xmlnsXlink="http://www.w3.org/1999/xlink"
                                 version="1.1" width={24} height={24}
                                 viewBox="0 0 24 24">
                                <path
                                    d="M2,4C2,2.89 2.9,2 4,2H7V4H4V7H2V4M22,4V7H20V4H17V2H20A2,2 0 0,1 22,4M20,20V17H22V20C22,21.11 21.1,22 20,22H17V20H20M2,20V17H4V20H7V22H4A2,2 0 0,1 2,20M10,2H14V4H10V2M10,20H14V22H10V20M20,10H22V14H20V10M2,10H4V14H2V10Z"/>
                            </svg>
                            <p>{this.props.task.squareMeters}<span>m2</span></p>
                        </div>
                        }
                    </div>
                </div>
            </div>


        );
    }
}

export default CleaningTaskElement;
//http://jsfiddle.net/weqm8q5w/6/