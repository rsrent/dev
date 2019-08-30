import React, {Component} from 'react';
import {Link} from "react-router-dom";
import AddFolder from "./AddFolder";
import UploadFile from "./UploadFile";
import HelperMethods from "./HelperMethods";


let azure = require('azure-storage/browser/azure-storage.blob.export.js');

let Config = require('./config.json');

class ShowFiles extends Component {
    constructor(props) {
        super(props);
        this.state = {
            fileUri: process.env.NODE_ENV === "production" ? "https://rentstorage.blob.core.windows.net" : "https://rentdevelopmentstorage.blob.core.windows.net/",
            SASToken: "",
            folders: {},
            blobs: [],
            loadedFolders: false,
            loadedBlobs: false,
            container: "",
            containerName: "",
            parentFolderID: null,
            addFolder: false,
            addFile: false,
            permission: JSON.parse(localStorage.getItem('permissions')).find(element => {
                return element.name.toLocaleLowerCase() === "document"
            })
            //We only need to concern ourselves with the document permission, so we find that from the permissions array
        };
        this.addFolder = this.addFolder.bind(this);
        this.addFile = this.addFile.bind(this);
        this.reloadFiles = this.reloadFiles.bind(this);
    }

    async componentDidMount() {
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        let fetchUrl = "";
        if (typeof this.props.match.params.folderID !== "undefined") {
            fetchUrl = "Document/GetForFolder/" + this.props.match.params.folderID;
            console.log("Getting for folder");
        } else if (typeof this.props.match.params.customerID !== "undefined" && typeof this.props.match.params.folderID === "undefined" && typeof this.props.match.params.locationID === "undefined") {
            fetchUrl = "Document/GetForCustomer/" + this.props.match.params.customerID;
            console.log("Getting for customer");
        } else if (typeof this.props.match.params.locationID !== "undefined" && typeof this.props.match.params.folderID === "undefined") {
            fetchUrl = "Document/GetForLocation/" + this.props.match.params.locationID;
            console.log("Getting for location");
        }
        HelperMethods.getRequest(fetchUrl, myHeaders,
            (result) => {
                console.log("Folders!", result);
                this.setState(
                    {
                        folders: result,
                        loadedFolders: true,
                        containerName: result.id + "-folder",
                        parentFolderID: result.parentFolderID
                    },
                    () => this.getSASToken());
            }, undefined, () => {
            });
    }

    async setRootID() {
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        await HelperMethods.getRequest("Document/GetForLocation/" + this.props.match.params.locationID, myHeaders,
            (result) => {
                localStorage.setItem("rootFolder", result.id);
                this.setState({rootID: result.id});
            }, undefined, () => {
            });
    }

    getSASToken() {
        if (this.state.containerName !== '0-folder') {


            let myHeaders = new Headers();
            myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
            HelperMethods.getRequest("Document/GetSASTokenDownload/" + this.state.containerName, myHeaders,
                (result) => {
                    this.setState(
                        {SASToken: result},
                        () => {
                            this.getFiles(this.state.containerName)
                        });
                }, undefined, () => {
                });
        }
        else {
            this.setState({loadedBlobs: true});
        }
    }

    getFiles(containerName) {


        let blobService = azure.createBlobServiceWithSas(this.state.fileUri, this.state.SASToken);
        blobService.listBlobsSegmented(containerName, null, (error, results) => {
            if (error) {
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

    addFolder() {
        this.setState({addFolder: !this.state.addFolder});
        this.setState({addFile: false});
    }

    addFile() {
        this.setState({addFile: !this.state.addFile});
        this.setState({addFolder: false});
    }

    reloadFiles() {
        this.setState({blobs: []});
        this.getFiles(this.state.containerName);
    }

    render() {
        let url = this.props.location.pathname.split('/');
        url = url.splice(0, url.length - 2);
        let pathName = url.join('/') + "/";
        return (
            <div>
                {this.state.folders.editable && this.state.folders.title !== "Opgavemappe" &&
                <button type="submit" className="btn btn-simp" onClick={this.addFolder}
                        style={{margin: '0 10px 0 0'}}>Tilføj folder
                </button>}
                {this.state.folders.editable &&
                <button type="submit" className="btn btn-simp" onClick={this.addFile}>Upload fil til mappe
                </button>}
                {this.state.addFolder && (this.state.folders.title !== "Opgavemappe" && this.state.folders.parent === null) &&
                <AddFolder
                    parentFolder={typeof this.props.match.params.folderID !== "undefined" && this.props.match.params.folderID !== "root" ? this.props.match.params.folderID : this.state.folders.id}/>
                }
                {this.state.addFile &&
                <UploadFile containerName={this.state.containerName} handler={this.reloadFiles}/>
                }
                <table style={{marginTop: '5px'}}>
                    <tbody>
                    <tr>
                        <th style={{width: '40px'}}/>
                        <th>{this.state.folders.title}</th>
                        <th style={{width: '40px'}}/>
                    </tr>
                    {typeof this.props.match.params.folderID !== "undefined" &&
                    <tr>
                        <td>⇧</td>
                        <td><Link to={{
                            pathname: `${pathName}${this.state.parentFolderID !== null ? this.state.parentFolderID + "/" : ""}`
                        }} href="#" title="Folder op">Folder op</Link></td>
                        <td/>
                    </tr>}
                    {this.state.loadedFolders &&
                    this.state.folders.documents.map(folder =>
                        folder.title !== "Lokationsmappe" && folder.title !== "Opgavemappe" || localStorage.getItem("roleID") !== "8" ?
                            <ShowFolderInfo folder={folder} userID={this.props.match.params.userID}
                                            locationID={this.props.match.params.locationID} key={folder.id}
                                            pathname={this.props.location.pathname}
                                            SASToken={this.state.SASToken} fileUri={this.state.fileUri}
                                            container={this.state.containerName} handler={this.reloadFiles}
                                            permission={this.state.permission} removable={folder.removable}
                                            folderID={this.props.match.params.folderID}/>
                            : <tr/>
                    )}
                    {this.state.loadedBlobs && this.state.blobs.map(blob =>
                        <ShowBlobInfo blob={blob} key={blob.id} SASToken={this.state.SASToken}
                                      fileUri={this.state.fileUri} container={this.state.containerName}
                                      pathname={this.props.location.pathname} folder={this.state.folders}
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
                <td><img src="img/file_icon.svg"/></td>
                <td><a href={this.props.blob.downloadLink}>{this.props.blob.name}</a></td>
                <td>{this.props.folder.editable &&
                <a href="#" onClick={this.deleteFile}><img src="img/delete.svg"/></a>
                }</td>
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

class ShowFolderInfo extends Component {
    constructor(props) {
        super(props);
        this.deleteFolder = this.deleteFolder.bind(this);

    }

    deleteFolder(event) {
        event.preventDefault();
        console.log(this.props.folder.title);
        if (window.confirm("Er du sikker på at du vil slette hele denne mappe og alle filer den indeholder?")) {
            let myHeaders = new Headers();
            myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
            return HelperMethods.deleteRequest("Document/" + this.props.folder.id, myHeaders,
                () => {
                    alert("Mappen blev fjernet.");
                    window.location.reload();
                }, undefined, () => {
                });
        }
    }

    render() {
        let url = this.props.pathname.split('/');
        url = url.splice(0, url.length - 2);
        let pathName = typeof this.props.folderID === "undefined" ? this.props.pathname : url.join('/') + "/";
        return (
            <tr>
                <td><img src="img/folder_icon.svg"/></td>
                <td><Link to={{
                    pathname: `${pathName}${this.props.folder.id}/`
                }} href="#">{this.props.folder.title}</Link></td>
                <td>{this.props.folder.editable && this.props.folder.removable &&
                <a href="#" onClick={this.deleteFolder}><img src="img/delete.svg"/></a>
                }</td>

            </tr>
        )
    }
}

export default ShowFiles;