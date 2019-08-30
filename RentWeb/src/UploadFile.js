import React, {Component} from 'react';

import {Line} from 'rc-progress';
import HelperMethods from "./HelperMethods";

const uuidv4 = require('uuid/v4');

let azure = require('azure-storage/browser/azure-storage.blob.export.js');

let Config = require('./config.json');

class UploadFile extends Component {
    constructor(props) {
        super(props);
        this.state = {
            fileUri: process.env.NODE_ENV === "production" ? "https://rentstorage.blob.core.windows.net" : "https://rentdevelopmentstorage.blob.core.windows.net/",
            SASToken: "",
            uploadProgress: -1,
            uploadDone: false,
            containerName: this.props.containerName,
            imageName: "",
            newImage: false
        };

        this.getSASToken = this.getSASToken.bind(this);
        this.handleChange = this.handleChange.bind(this);
    }

    //fileService = azure.createFileServiceWithSas(this.fileUri, 'SAS_TOKEN');

    componentDidMount(){
        if(typeof this.props.imageName !== 'undefined'){
            this.setState({imageName: this.props.imageName});
        }
        if(this.props.newImage !== undefined){
            this.setState({newImage: this.props.newImage});
        }
    }

    getSASToken(event) {
        event.preventDefault();
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        HelperMethods.getRequest("Document/GetSASTokenUpload/" + this.state.containerName, myHeaders,
            (result) => {
                this.setState(
                    {SASToken: result},
                    () => {
                        this.uploadFile()
                    });
            });
        // fetch(Config.API_URL + "/Document/GetSASTokenUpload/" + this.state.containerName, {
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
        //                 this.uploadFile()
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

    finishedOrError = false;
    speedSummary = null;

    uploadFile() {
        this.setState({uploadDone: false});
        this.setState({uploadProgress: -1});


        // If one file has been selected in the HTML file input element
        let file = this.fileInput.files[0];
        if(file.type !== "application/pdf" && !file.type.startsWith("image/") && file.type !== "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && file.type !== "application/vnd.ms-excel"){
            alert("Der kan kun uploades billeder, PDF og Excel filer.");
            return;
        }
        console.log("FILE", file);
        console.log("SAS TOKEN", this.state.SASToken);
        let fileName = "";
        console.log("newIMGAGE", this.state.newImage);
        if(this.state.newImage){
            fileName = uuidv4();
        }else{
            fileName = this.state.imageName === "" ? file.name : this.state.imageName;
        }


        let blobService = azure.createBlobServiceWithSas(this.state.fileUri, this.state.SASToken);

        let customBlockSize = file.size > 1024 * 1024 * 32 ? 1024 * 1024 * 4 : 1024 * 512;
        blobService.singleBlobPutThresholdInBytes = customBlockSize;
        console.log("ContainerName", this.state.containerName);

        this.speedSummary = blobService.createBlockBlobFromBrowserFile(this.state.containerName, fileName, file, {blockSize: customBlockSize}, (error, result, response) => {
            this.finishedOrError = true;
            console.log(result, response);
            if (error) {
                console.log(error);
                alert("Der skete en fejl under upload.");
            } else {
                this.setState({uploadDone: true});
                if(this.state.newImage){
                    console.log("Calling handler with", fileName);
                    this.props.handler(fileName);
                }else{
                    this.props.handler();
                }
            }
        });
        this.refreshProgress();
    }

    refreshProgress() {
        setTimeout(() => {
            if (!this.finishedOrError) {
                let process = this.speedSummary.getCompletePercent();
                console.log(process);
                this.setState({uploadProgress: process});
                this.refreshProgress();
            }
        }, 200);
        let process = this.speedSummary.getCompletePercent();
        console.log(process);
        this.setState({uploadProgress: process});
    }

    handleChange(){
        document.getElementById('fileSpan').innerText = this.fileInput.files[0].name;
    }

    render() {
        let loadStyle = {
            width: '30%'
        };
        return (
            <div style={{marginBottom:'20px'}}>
                {this.state.uploadProgress >= 0 && !this.state.uploadDone &&
                <Line percent={this.state.uploadProgress} strokeWidth="1" strokeColor="#005AAD" style={loadStyle}/>
                }
                {this.state.uploadDone &&
                <Line percent="100" strokeWidth="1" strokeColor="#005AAD" style={loadStyle}/>
                }
                {this.state.uploadDone &&
                <h2>Upload succesfuldt!</h2>
                }
                <form onSubmit={this.getSASToken} style={{marginTop:'20px'}}>
                    <label className="custom-file"  style={{marginBottom:'20px', width:'30%'}}>
                        <input type="file"
                               ref={input => {
                                   this.fileInput = input;
                               }} id="file" className="custom-file-input" onChange={this.handleChange} accept="image/*, application/pdf, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel"/>
                        <span className="custom-file-control" id="fileSpan" >Vælg fil</span>
                    </label>
                    <button type="submit" className="btn btn-simp select-button btn-primary" style={{width: '30%', marginBottom:'20px', clear:'both', display:'block'}}>Upload</button>
                </form>
            </div>


        );
    }
}

export default UploadFile;