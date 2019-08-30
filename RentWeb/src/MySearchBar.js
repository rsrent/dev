import React, {Component} from 'react';

class MySearchBar extends Component {
    constructor(props) {
        super(props);
    }


    render() {
        return (
            <input name="searchBar" placeholder="Søg" className="searchBar" type="text" onChange={(e) => { this.props.handler({ searchText: e.target.value }); }}/>
        );
    }
}

export default MySearchBar;