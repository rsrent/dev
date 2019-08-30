import React, {Component} from 'react';
import {Link, Redirect} from "react-router-dom";
import UploadFile from "./UploadFile";
import LoadingIndicator from "./LoadingIndicator";

let Config = require('./config.json');

class UserProfile extends React.Component {
    constructor(props) {
        super(props);
        if (typeof this.props.location.params === 'undefined') {
            this.state = {
                person: null,
                loaded: false,
                redirect: false
            };
            this.getImageLocation();
        } else {
            this.state = {
                person: this.props.location.params.person,
                loaded: true,
                redirect: false
            }
        }
        this.setImageLocation = this.setImageLocation.bind(this);
    }

    getImageLocation() {
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        fetch(Config.API_URL + "/Users/" + this.props.match.params.profileID, {
            method: "GET",
            headers: myHeaders
        })
            .then(result => {
                if (!result.ok) {
                    throw result;
                }
                return result.json()
            })
            .then(user => {
                this.setState({person: user, loaded: true});
            })
            .catch(error => {
                console.error(error);
                if (error.status === 404) {
                    alert("Not found");
                } else if (error.status === 401) {
                    //localStorage.clear();
                    //window.location.replace("/");
                } else {
                    alert("Der skete en ukendt serverfejl");
                }
            });
    }

    setImageLocation(newImageLocation = null) {
        if (newImageLocation === null || newImageLocation === this.state.person.imageLocation) {
            return;
        }
        console.log("newimage", newImageLocation);
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        fetch(Config.API_URL + "/Users/UpdateUserImage/" + this.state.person.id + "/" + newImageLocation, {
            method: "PUT",
            headers: myHeaders
        })
            .then(result => {
                if (!result.ok) {
                    throw result;
                }
                alert("Profilbilledet blev skiftet");
                this.setState({redirect: true})
            })
            .catch(error => {
                console.error(error);
                if (error.status === 404) {
                    alert("Not found");
                } else if (error.status === 401) {
                    //localStorage.clear();
                    ////window.location.replace("/");
                } else {
                    alert("Der skete en ukendt serverfejl");
                }
            });
    }

    render() {
        if (this.state.redirect) {
            return (
                <Redirect to={"/Locations/User/" + this.props.match.params.userID + "/" + this.props.match.params.locationID + "/Team"}/>
            );
        }
        return (
            <div>
                {this.state.loaded ?
                    <div>
                        <h2>{this.state.person.firstName} {this.state.person.lastName}</h2>
                        <p>Her kan du skifte profilbillede</p>
                        <hr />
                        {this.state.person.imageLocation === null
                            ? <UploadFile containerName="image" handler={this.setImageLocation} newImage={true} />
                            : <UploadFile containerName="image" handler={this.setImageLocation}
                                          imageName={this.state.person.imageLocation} newImage={false} />
                        }
                    </div>
                    :
                    <LoadingIndicator/>
                }
            </div>



        );
    }
}

export default UserProfile;
