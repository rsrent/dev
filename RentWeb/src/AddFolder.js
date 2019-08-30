import React, {Component} from 'react';
import HelperMethods from "./HelperMethods";

let azure = require('azure-storage/browser/azure-storage.blob.export.js');

let Config = require('./config.json');

class AddFolder extends Component {
    constructor(props) {
        super(props);
        this.state = {inputFolderName: ""};
        this.handleInputChange = this.handleInputChange.bind(this);
        this.addFolder = this.addFolder.bind(this);
    }

    addFolder(event){
        event.preventDefault();
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        HelperMethods.genericRequestWithoutBody("Document/AddFolder/" + this.props.parentFolder + "/" + this.state.inputFolderName, myHeaders, () => {
            alert("Folder tilføjet!");
            window.location.reload();
        }, undefined, "POST");
        // fetch(Config.API_URL + "/Document/AddFolder/" + this.props.parentFolder + "/" + this.state.inputFolderName, {
        //     method: "POST",
        //     headers: myHeaders
        // })
        //     .then(result => {
        //         if (!result.ok) {
        //             throw result;
        //         }
        //         return result.json()
        //     })
        //     .then(() => {
        //         alert("Folder tilføjet!");
        //         window.location.reload();
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

    handleInputChange(event) {
        const name = event.target.name;
        this.setState({[name]: event.target.value});
    }

    render() {
        return (
            <div style={{marginBottom:'20px'}}>
                <form onSubmit={this.addFolder} style={{marginTop:'20px'}}>
                    <label style={{marginBottom:'20px', width:'30%'}}>
                        <input type="text" id="inputFolderName" className="form-control"
                               placeholder="Folder navn" pattern="^[a-zA-Z0-9 æøåÆØÅ]+$"
                               title="Foldernavnet må kun indeholde bogstaver, tal og mellemrum"
                               tabIndex={1} required
                               name="inputFolderName"
                               value={this.state.inputFolderName} onChange={this.handleInputChange}
                        />
                    </label>
                    <button type="submit" className="btn btn-simp select-button btn-primary" style={{width: '30%', marginBottom:'20px', clear:'both', display:'block'}}>Opret folder</button>

                </form>
            </div>


        );
    }
}

export default AddFolder;