import React, {Component} from 'react';
import {Link} from "react-router-dom";
import AddFolder from "./AddFolder";
import UploadFile from "./UploadFile";
import HelperMethods from "./HelperMethods";


let azure = require('azure-storage/browser/azure-storage.blob.export.js');

let Config = require('./config.json');

class ShowBlobs extends Component {
    constructor(props) {
        super(props);
        this.state = {
            fileUri: process.env.NODE_ENV === "production" ? "https://rentstorage.blob.core.windows.net" : "https://rentdevelopmentstorage.blob.core.windows.net/",
            SASToken: "",
            blobs: [],
            loadedBlobs: false,
            container: "",
            containerName: "",
            permission: JSON.parse(localStorage.getItem('permissions')).find(element => {
                return element.name.toLocaleLowerCase() === "document"
            })
        };
    }

    async componentDidMount() {
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        this.getSASToken();
    }

    getSASToken() {
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        HelperMethods.getRequest("Document/GetSASTokenDownload/" + this.props.containerName, myHeaders,
            (result) => {
                console.log(result);
                this.setState(
                    {SASToken: result},
                    () => {
                        this.getFiles(this.props.containerName)
                    });
            });
    }

    getFiles(containerName) {
        let blobService = azure.createBlobServiceWithSas(this.state.fileUri, this.state.SASToken);
        blobService.listBlobsSegmented(containerName, null, (error, results) => {
            if (error) {
                this.setState({loadedBlobs:true});
                // List blobs error
            } else {
                for (let i = 0, blob; blob = results.entries[i]; i++) {
                    blob.downloadLink = blobService.getUrl(containerName, blob.name, this.state.SASToken);
                    blob.downloadLink = blob.downloadLink.replace("%3F", ""); // For some reason an extra question mark is inserted from the API. This removes the URL encoded question mark.
                }
                this.setState({loadedBlobs: true});

                this.setState({blobs: results.entries});
            }
        });
    }


    render() {
        return (
            <div>
                <table style={{marginTop: '5px'}}>
                    <tbody>
                    {this.state.loadedBlobs && this.state.blobs.map(blob =>
                        <ShowBlobInfo blob={blob} key={blob.id} SASToken={this.state.SASToken}
                                      fileUri={this.state.fileUri} container={this.state.containerName}
                                      handler={this.reloadFiles} permission={this.state.permission}/>
                    )}
                    {!this.state.loadedFolders || !this.state.loadedBlobs &&
                    <tr>
                        <td/>
                        <td>Loading...</td>
                        <td/>
                    </tr>}
                    </tbody>
                </table>
            </div>


        );
    }

}

class ShowBlobInfo extends Component {
    constructor(props) {
        super(props);
        this.deleteFile = this.deleteFile.bind(this);
        console.log(this.props.blob);
    }

    deleteFile(event) {
        event.preventDefault();
        console.log("blob", this.props.blob);
        if (window.confirm("Er du sikker på at du vil slette denne fil?")) {
            let blobService = azure.createBlobServiceWithSas(this.props.fileUri, this.props.SASToken);
            blobService.deleteBlobIfExists(this.props.container, this.props.blob.name, (error, result) => {
                console.log("Delete blob result", result);
                if (error) {
                    alert("Kunne ikke fjerne filen");
                } else {
                    alert("Filen blev fjernet");
                    this.props.handler();
                }
            });
        }
    }

    render() {
        return (
            <tr>
                <td style={{width: '40px'}}><img src="img/file_icon.svg"/></td>
                <td><a href={this.props.blob.downloadLink}>{this.props.blob.name}</a></td>
            </tr>
        )
    }


    humanFileSize(bytes, si) {
        let thresh = si ? 1000 : 1024;
        if (Math.abs(bytes) < thresh) {
            return bytes + ' B';
        }
        let units = si
            ? ['kB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB']
            : ['KiB', 'MiB', 'GiB', 'TiB', 'PiB', 'EiB', 'ZiB', 'YiB'];
        let u = -1;
        do {
            bytes /= thresh;
            ++u;
        } while (Math.abs(bytes) >= thresh && u < units.length - 1);
        return bytes.toFixed(1) + ' ' + units[u];
    }
}


export default ShowBlobs;