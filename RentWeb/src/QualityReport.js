import React, {Component} from 'react';
import HelperMethods from "./HelperMethods";
import LoadingIndicator from "./LoadingIndicator";

let Config = require('./config.json');


class QualityReport extends Component {
    constructor(props) {
        super(props);
        if (props.location.state !== undefined) {
            this.state = {
                reportID: props.match.params.reportID,
                floorID: props.match.params.floorID,
                areas: props.location.state.areas,
                loaded: true
            }
        } else {
            this.state = {
                reportID: props.match.params.reportID,
                floorID: props.match.params.floorID,
                areas: [],
                loaded: false
            };
            this.getAreas();
        }
    }

    getAreas() {
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        HelperMethods.getRequest("QualityReports/GetPlanWithFloors/" + this.state.reportID, myHeaders,
            (report) => {
                this.setState({areas: report.floors.filter(floor => parseInt(floor.floor.id) === parseInt(this.state.floorID))[0].areas});
            }, undefined, () => {
                this.setState({loaded:true});
            });
    }

    render() {
        return (
            <main role="main" className="container-fluid">
                <div className="row">
                    <div className="col-10" style={{margin: 'auto'}}>
                        <div className="main-content-wrapper">
                            {this.state.loaded ?
                                this.state.areas.map(area =>
                                    <div key={area.area.id}>
                                        {area.qualityReportItems.map(qualityItem =>
                                            <div className="quality-report-card-content">
                                                <div className="quality-report-card" id="quality-report-card2">
                                                    <div className="quality-report-card-header">
                                                        <div className="quality-report-card-title">
                                                            <svg xmlns="http://www.w3.org/2000/svg"
                                                                 xmlnsXlink="http://www.w3.org/1999/xlink" version="1.1"
                                                                 width={24} height={24} viewBox="0 0 24 24">
                                                                <path
                                                                    d="M17.63,5.84C17.27,5.33 16.67,5 16,5H5A2,2 0 0,0 3,7V17A2,2 0 0,0 5,19H16C16.67,19 17.27,18.66 17.63,18.15L22,12L17.63,5.84Z"/>
                                                            </svg>
                                                            <h2>{area.area.description}{qualityItem.cleaningTask.comment !== null && qualityItem.cleaningTask.comment !== "" && " - " + qualityItem.cleaningTask.comment}</h2>
                                                        </div>
                                                    </div>
                                                    <div
                                                        className="quality-report-card-details d-flex justify-content-between">
                                                        <div className="quality-report-card-details-code">
                                                            <p>{qualityItem.cleaningTask.frequency}</p>
                                                        </div>
                                                        <div className="quality-report-card-details-size">
                                                            <svg xmlns="http://www.w3.org/2000/svg"
                                                                 xmlnsXlink="http://www.w3.org/1999/xlink" version="1.1"
                                                                 width={24} height={24} viewBox="0 0 24 24">
                                                                <path
                                                                    d="M2,4C2,2.89 2.9,2 4,2H7V4H4V7H2V4M22,4V7H20V4H17V2H20A2,2 0 0,1 22,4M20,20V17H22V20C22,21.11 21.1,22 20,22H17V20H20M2,20V17H4V20H7V22H4A2,2 0 0,1 2,20M10,2H14V4H10V2M10,20H14V22H10V20M20,10H22V14H20V10M2,10H4V14H2V10Z"/>
                                                            </svg>
                                                            <p>{qualityItem.cleaningTask.squareMeters}<span>m2</span>
                                                            </p>
                                                        </div>
                                                        {/* RATING */}
                                                        <form className="quality-report-card-details-rating"
                                                              id="rating-group2">
                                                            {/* HAPPY */}
                                                            <label htmlFor="happy2" className="emoticon">
                                                                <input type="radio" name="rating" className="happy"
                                                                       id="happy2"
                                                                       defaultValue="happy"
                                                                       defaultChecked={qualityItem.rating === 1}
                                                                       disabled={qualityItem.rating !== 1}/>
                                                                <svg xmlns="http://www.w3.org/2000/svg"
                                                                     xmlnsXlink="http://www.w3.org/1999/xlink"
                                                                     version="1.1"
                                                                     width="100%" height="100%" viewBox="0 0 24 24">
                                                                    <path
                                                                        d="M20,12A8,8 0 0,0 12,4A8,8 0 0,0 4,12A8,8 0 0,0 12,20A8,8 0 0,0 20,12M22,12A10,10 0 0,1 12,22A10,10 0 0,1 2,12A10,10 0 0,1 12,2A10,10 0 0,1 22,12M10,9.5C10,10.3 9.3,11 8.5,11C7.7,11 7,10.3 7,9.5C7,8.7 7.7,8 8.5,8C9.3,8 10,8.7 10,9.5M17,9.5C17,10.3 16.3,11 15.5,11C14.7,11 14,10.3 14,9.5C14,8.7 14.7,8 15.5,8C16.3,8 17,8.7 17,9.5M12,17.23C10.25,17.23 8.71,16.5 7.81,15.42L9.23,14C9.68,14.72 10.75,15.23 12,15.23C13.25,15.23 14.32,14.72 14.77,14L16.19,15.42C15.29,16.5 13.75,17.23 12,17.23Z"/>
                                                                </svg>
                                                            </label>
                                                            {/* /HAPPY */}
                                                            {/* NEUTRAL */}
                                                            <label htmlFor="neutral2" className="emoticon">
                                                                <input type="radio" name="rating" className="neutral"
                                                                       id="neutral2" defaultValue="neutral"
                                                                       defaultChecked={qualityItem.rating === 2}
                                                                       disabled={qualityItem.rating !== 2}/>
                                                                <svg xmlns="http://www.w3.org/2000/svg"
                                                                     xmlnsXlink="http://www.w3.org/1999/xlink"
                                                                     version="1.1"
                                                                     width="100%" height="100%" viewBox="0 0 24 24">
                                                                    <path
                                                                        d="M8.5,11A1.5,1.5 0 0,1 7,9.5A1.5,1.5 0 0,1 8.5,8A1.5,1.5 0 0,1 10,9.5A1.5,1.5 0 0,1 8.5,11M15.5,11A1.5,1.5 0 0,1 14,9.5A1.5,1.5 0 0,1 15.5,8A1.5,1.5 0 0,1 17,9.5A1.5,1.5 0 0,1 15.5,11M12,20A8,8 0 0,0 20,12A8,8 0 0,0 12,4A8,8 0 0,0 4,12A8,8 0 0,0 12,20M12,2A10,10 0 0,1 22,12A10,10 0 0,1 12,22C6.47,22 2,17.5 2,12A10,10 0 0,1 12,2M9,14H15A1,1 0 0,1 16,15A1,1 0 0,1 15,16H9A1,1 0 0,1 8,15A1,1 0 0,1 9,14Z"/>
                                                                </svg>
                                                            </label>
                                                            {/* /NEUTRAL */}
                                                            {/* SAD */}
                                                            <label htmlFor="sad2" className="emoticon">
                                                                <input type="radio" name="rating" className="sad"
                                                                       id="sad2"
                                                                       defaultValue="sad"
                                                                       defaultChecked={qualityItem.rating === 3}
                                                                       disabled={qualityItem.rating !== 3}/>
                                                                <svg xmlns="http://www.w3.org/2000/svg"
                                                                     xmlnsXlink="http://www.w3.org/1999/xlink"
                                                                     version="1.1"
                                                                     width="100%" height="100%" viewBox="0 0 24 24">
                                                                    <path
                                                                        d="M20,12A8,8 0 0,0 12,4A8,8 0 0,0 4,12A8,8 0 0,0 12,20A8,8 0 0,0 20,12M22,12A10,10 0 0,1 12,22A10,10 0 0,1 2,12A10,10 0 0,1 12,2A10,10 0 0,1 22,12M15.5,8C16.3,8 17,8.7 17,9.5C17,10.3 16.3,11 15.5,11C14.7,11 14,10.3 14,9.5C14,8.7 14.7,8 15.5,8M10,9.5C10,10.3 9.3,11 8.5,11C7.7,11 7,10.3 7,9.5C7,8.7 7.7,8 8.5,8C9.3,8 10,8.7 10,9.5M12,14C13.75,14 15.29,14.72 16.19,15.81L14.77,17.23C14.32,16.5 13.25,16 12,16C10.75,16 9.68,16.5 9.23,17.23L7.81,15.81C8.71,14.72 10.25,14 12,14Z"/>
                                                                </svg>
                                                            </label>
                                                            {/* /SAD */}
                                                        </form>
                                                        {/* /RATING */}
                                                    </div>
                                                    {qualityItem.comment !== null &&
                                                    <div className="quality-report-card-comment-wrapper">
                                                        <div className="quality-report-card-comment">
                                                            <p>{qualityItem.comment}</p>
                                                        </div>
                                                    </div>
                                                    }
                                                </div>
                                            </div>
                                        )}
                                    </div>
                                )
                                :
                                <LoadingIndicator/>}
                        </div>
                    </div>
                </div>
            </main>

        );
    }
}

export default QualityReport;