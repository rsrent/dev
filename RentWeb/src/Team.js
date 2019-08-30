import React, {Component} from 'react';
import {Link} from "react-router-dom";
import HelperMethods from "./HelperMethods";
import LoadingIndicator from "./LoadingIndicator";

let azure = require('azure-storage/browser/azure-storage.blob.export.js');

let Config = require('./config.json');


class Team extends Component {
    constructor(props) {
        super(props);
        this.state = {
            locationID: props.match.params.locationID,
            people: [],
            fileUri: process.env.NODE_ENV === "production" ? "https://rentstorage.blob.core.windows.net" : "https://rentdevelopmentstorage.blob.core.windows.net/",
            SASToken: "",
            blobs: [],
            loaded: false,
            loadedBlobs: false
        };
    }

    uploadPermission = JSON.parse(localStorage.getItem('permissions')).find(element => {
        return element.name.toLocaleLowerCase() === "user"
    });

    componentDidMount() {
        this.getSASToken();
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        HelperMethods.getRequest("Users/GetLocationUsers/" + this.state.locationID, myHeaders,
            (people) => {
                this.setState({people: people, loaded: true});
            });
        // fetch(Config.API_URL + "/Users/GetLocationUsers/" + this.state.locationID, {
        //     method: "GET",
        //     headers: myHeaders
        // })
        //     .then(result => {
        //         if (!result.ok) {
        //             throw result;
        //         }
        //         return result.json()
        //     })
        //     .then(people => {
        //         this.setState({people: people, loaded: true});
        //     })
        //     .catch(error => {
        //         console.error("ERROOOOR", error);
        //         if (error.status === 404) {
        //             alert("Den givne lokation findes ikke");
        //         } else if (error.status === 401) {
        //             localStorage.clear();
        //             window.location.replace("/");
        //         } else {
        //             alert("Der skete en ukendt serverfejl");
        //         }
        //     });
    }

    // getImage(imageID) {
    //     let image = null;
    //     if (imageID === null) return;
    //     let myHeaders = new Headers();
    //     myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
    //     fetch(Config.API_URL + '/Image/resize/' + imageID + '/150', {
    //         method: "GET",
    //         headers: myHeaders
    //     })
    //         .then(function (response) {
    //             image = response.blob();
    //         });
    //     return image;
    // }

    getSASToken() {
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        HelperMethods.getRequest("Document/GetSASTokenDownload/thumbnail-150", myHeaders,
            (result) => {
                this.setState(
                    {SASToken: result},
                    () => {
                        this.getFiles()
                    });
            }, undefined, () => {
            });
        // fetch(Config.API_URL + "/Document/GetSASTokenDownload/thumbnail-150", {
        //     method: "GET",
        //     headers: myHeaders
        // })
        //     .then(result => {
        //         if (!result.ok) {
        //             throw result;
        //         }
        //         return result.json()
        //     })
        //     .then(result => {
        //         console.log(result);
        //         this.setState(
        //             {SASToken: result},
        //             () => {
        //                 this.getFiles()
        //             });
        //     })
        //     .catch(error => {
        //         console.error(error);
        //         if (error.status === 404) {
        //             alert("Not found");
        //         } else if (error.status === 401) {
        //             localStorage.clear();
        //             window.location.replace("/");
        //         } else {
        //             alert("Der skete en ukendt serverfejl");
        //         }
        //     });
    }

    getFiles() {
        let containerName = "thumbnail-150";
        let blobService = azure.createBlobServiceWithSas(this.state.fileUri, this.state.SASToken);
        blobService.listBlobsSegmented(containerName, null, (error, results) => {
            if (error) {
                console.log(error);
            } else {
                for (let i = 0, blob; blob = results.entries[i]; i++) {
                    blob.downloadLink = blobService.getUrl(containerName, blob.name, this.state.SASToken);
                    blob.downloadLink = blob.downloadLink.replace("%3F", ""); // For some reason an extra question mark is inserted from the API. This removes the URL encoded question mark.
                    let person = this.state.people.find(person => {
                        return person.imageLocation === blob.name
                    });
                    if (person !== undefined) {
                        person.downloadLink = blob.downloadLink;
                    }
                }
                this.setState({loadedBlobs: true});

                this.setState({blobs: results.entries});
            }
        });

    }

    render() {
        return (
            <div className="buttons-group">
                {
                    this.state.loaded && this.state.people.length ?
                        this.state.people.map(person =>
                            this.uploadPermission.update
                                ? <Link to={{
                                    pathname: `${this.props.location.pathname}${person.id}/`,
                                    params: {person: person}
                                }}>
                                    <TeamCard person={person}/>
                                </Link>
                                : <TeamCard person={person}/>
                        )
                        :
                        this.state.loaded
                            ? <p>Der er ikke tilknyttet nogen ansatte til denne lokation.</p>
                            : <LoadingIndicator/>
                }
            </div>
        );
    }

}

class TeamCard extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        let person = this.props.person;
        return (
            <div className="team-card-content">
                <div className="team-member-card">
                    <div className="team-member-photo-wrapper" style={person.downloadLink !== undefined ? {
                        backgroundSize: 'cover',
                        backgroundPosition: 'center center',
                        backgroundImage: 'url("' + person.downloadLink + '")',
                    } : {}}>
                        {person.downloadLink === undefined && <div className="team-member-photo-placeholder"/>}
                    </div>
                    <div className="team-member-details">
                        <div className="team-member-name">
                            <h2>{person.firstName} {person.lastName}</h2>
                        </div>
                        <div className="team-member-role">
                            <p className="lead">{person.title}</p>
                        </div>
                        {person.hourText !== null &&
                        <div>
                            <p>{person.hourText}</p>
                        </div>}
                    </div>
                </div>
            </div>
        );
    }
}

export default Team;
