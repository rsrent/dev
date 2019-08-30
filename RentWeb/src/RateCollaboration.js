import React, {Component} from 'react';
import {Link} from "react-router-dom";
import AddFolder from "./AddFolder";
import UploadFile from "./UploadFile";
import HelperMethods from "./HelperMethods";


let azure = require('azure-storage/browser/azure-storage.blob.export.js');

let Config = require('./config.json');

class RateCollaboration extends Component {
    constructor(props) {
        super(props);

        this.state = {comment: "", rating: 1};

        this.handleInputChange = this.handleInputChange.bind(this);
        this.sendRating = this.sendRating.bind(this);
    }

    sendRating(event) {
        event.preventDefault();

        let title = "Customer opinion of collaboration";
        let comment = JSON.stringify(this.state.comment);
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        myHeaders.append("Content-Type", "application/json");
        HelperMethods.postRequest("Rating/AddRatingItem/" + this.props.match.params.ratingID + "/" + title + "/" + this.state.rating, myHeaders,
            () => {
                window.location.replace("/");
                alert("Tak for din mening.");
            },
            (error) => {
                console.error("Error-: " + error);
                if (error.status === 404) {
                    alert("Der er ikke nogen rating med dette ID");
                } else if (error.status === 403) {
                    alert("Din session er udløbet. Login igen");
                    localStorage.clear();
                    window.location.replace("/");
                } else if (error.status === 401) {
                    alert("Du har adgang til denne funktion.");
                } else {
                    alert("Der skete en ukendt serverfejl");
                }
            }, comment);
        // fetch(Config.API_URL + "/Rating/AddRatingItem/" + this.props.match.params.ratingID + "/" + title + "/" + this.state.rating,
        //     {
        //         method: "POST",
        //         body: comment,
        //         headers: myHeaders
        //     })
        //     .then(function (res) {
        //         if (!res.ok) {
        //             throw res;
        //         }
        //         window.location.replace("/");
        //         alert("Tak for din mening.");
        //     })
        //     .then(data => {
        //         console.log(data);
        //     })
        //     .catch(error => {
        //         if (error.status === 401) {
        //             //localStorage.clear();
        //             //window.location.replace("/");
        //         } else if(error.status === 404) {
        //             alert("Der er ikke nogen rating med dette ID");
        //         }else{
        //             alert("Der skete en ukendt serverfejl");
        //         }
        //     });
    }

    handleInputChange(event) {
        const name = event.target.name;
        this.setState({[name]: event.target.value});
    }

    render() {
        let labelStyle = {width: '100px', height: '100px'};
        return (
            <div className="quality-report-card-content">
                <div className="quality-report-card" id="quality-report-card1">
                    <div className="quality-report-card-header">
                        <div className="quality-report-card-title">
                            <h2>Hvordan vil du bedømme vores samarbejde</h2>
                        </div>
                    </div>
                    <div className="quality-report-card-details d-flex justify-content-between"
                         style={{margin: 'auto', height: '110px'}}>
                        {/* RATING */}
                        <form className="quality-report-card-details-rating" id="rating-group1">
                            {/* HAPPY */}
                            <label htmlFor="happy1" className="emoticon" style={labelStyle}>
                                <input type="radio" name="rating" className="happy" id="happy1" defaultValue="1"
                                       defaultChecked value="1" onChange={this.handleInputChange}/>
                                <svg xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink"
                                     version="1.1" width="100px" height="100px" viewBox="0 0 24 24">
                                    <path
                                        d="M20,12A8,8 0 0,0 12,4A8,8 0 0,0 4,12A8,8 0 0,0 12,20A8,8 0 0,0 20,12M22,12A10,10 0 0,1 12,22A10,10 0 0,1 2,12A10,10 0 0,1 12,2A10,10 0 0,1 22,12M10,9.5C10,10.3 9.3,11 8.5,11C7.7,11 7,10.3 7,9.5C7,8.7 7.7,8 8.5,8C9.3,8 10,8.7 10,9.5M17,9.5C17,10.3 16.3,11 15.5,11C14.7,11 14,10.3 14,9.5C14,8.7 14.7,8 15.5,8C16.3,8 17,8.7 17,9.5M12,17.23C10.25,17.23 8.71,16.5 7.81,15.42L9.23,14C9.68,14.72 10.75,15.23 12,15.23C13.25,15.23 14.32,14.72 14.77,14L16.19,15.42C15.29,16.5 13.75,17.23 12,17.23Z"/>
                                </svg>
                            </label>
                            {/* /HAPPY */}
                            {/* NEUTRAL */}
                            <label htmlFor="neutral1" className="emoticon" style={labelStyle}>
                                <input type="radio" name="rating" className="neutral" id="neutral1"
                                       defaultValue="2" value="2" onChange={this.handleInputChange}/>
                                <svg xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink"
                                     version="1.1" width="100px" height="100px" viewBox="0 0 24 24">
                                    <path
                                        d="M8.5,11A1.5,1.5 0 0,1 7,9.5A1.5,1.5 0 0,1 8.5,8A1.5,1.5 0 0,1 10,9.5A1.5,1.5 0 0,1 8.5,11M15.5,11A1.5,1.5 0 0,1 14,9.5A1.5,1.5 0 0,1 15.5,8A1.5,1.5 0 0,1 17,9.5A1.5,1.5 0 0,1 15.5,11M12,20A8,8 0 0,0 20,12A8,8 0 0,0 12,4A8,8 0 0,0 4,12A8,8 0 0,0 12,20M12,2A10,10 0 0,1 22,12A10,10 0 0,1 12,22C6.47,22 2,17.5 2,12A10,10 0 0,1 12,2M9,14H15A1,1 0 0,1 16,15A1,1 0 0,1 15,16H9A1,1 0 0,1 8,15A1,1 0 0,1 9,14Z"/>
                                </svg>
                            </label>
                            {/* /NEUTRAL */}
                            {/* SAD */}
                            <label htmlFor="sad1" className="emoticon" style={labelStyle}>
                                <input type="radio" name="rating" className="sad" id="sad1" defaultValue="3"
                                       value="3" onChange={this.handleInputChange}/>
                                <svg xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink"
                                     version="1.1" width="100px" height="100px" viewBox="0 0 24 24">
                                    <path
                                        d="M20,12A8,8 0 0,0 12,4A8,8 0 0,0 4,12A8,8 0 0,0 12,20A8,8 0 0,0 20,12M22,12A10,10 0 0,1 12,22A10,10 0 0,1 2,12A10,10 0 0,1 12,2A10,10 0 0,1 22,12M15.5,8C16.3,8 17,8.7 17,9.5C17,10.3 16.3,11 15.5,11C14.7,11 14,10.3 14,9.5C14,8.7 14.7,8 15.5,8M10,9.5C10,10.3 9.3,11 8.5,11C7.7,11 7,10.3 7,9.5C7,8.7 7.7,8 8.5,8C9.3,8 10,8.7 10,9.5M12,14C13.75,14 15.29,14.72 16.19,15.81L14.77,17.23C14.32,16.5 13.25,16 12,16C10.75,16 9.68,16.5 9.23,17.23L7.81,15.81C8.71,14.72 10.25,14 12,14Z"/>
                                </svg>
                            </label>
                            {/* /SAD */}
                        </form>
                        {/* /RATING */}
                    </div>
                    {/* COMMENT */}
                    <div className="quality-report-card-comment-wrapper">
                        <div className="quality-report-card-comment">
                            <textarea placeholder="Kommentar(Valgfri)"
                                      style={{marginTop: "5px", width: "100%", fontSize: "11pt", height: '100px'}}
                                      name="comment" value={this.state.comment} onChange={this.handleInputChange}/>
                        </div>
                    </div>
                    {/* /COMMENT */}
                    <button type="submit" className="btn btn-simp select-button" style={{borderRadius: 0}} onClick={this.sendRating}>Bedøm
                    </button>
                </div>
            </div>



        );
    }

}

export default RateCollaboration;
