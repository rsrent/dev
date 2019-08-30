import React, { Component } from 'react';
import {Link} from "react-router-dom";
import { LocationCell } from './LocationCell'
import { SearchBar } from '../search/SearchBar'
import { GetLocations } from '../api/API.js'
import '../list-style.css';

export class LocationList extends Component {
 constructor(props) {
   super(props);

   this.state = {
     searchText: '',
     searchOptions: ['Name', 'Address'],
     searchOption: 'Name',
     sortOption: 'Name',
     sortReversed: false,
     isLoading: false,
     locations: [],
   }

   this.updateState = this.updateState.bind(this);
   this.handleChange = this.handleChange.bind(this);
   this.getFilteredAndSortedLocations = this.getFilteredAndSortedLocations.bind(this);
   this.compareForSort = this.compareForSort.bind(this);
 }

 updateState(obj) { this.setState(obj); }

 handleChange(event) {

   let name = event.target.name;
   let value = event.target.value;

   if(name === 'searchOption') { this.setState({ searchOption: value }); }
   if(name === 'sortOption') { this.setState({ sortOption: value }); }
   if(name === 'searchText') { this.setState({ searchText: value }); }
   if(name === 'reversedSearch') { this.setState({ sortReversed: !this.state.sortReversed }); }
 }

 getFilteredAndSortedLocations() {
   let locations = this.state.locations;
   let searchOption = this.state.searchOption;
   let searchText = this.state.searchText;
   return locations.filter(l =>
     (searchOption === 'Name' && (l.name).toLowerCase().includes(searchText.toLowerCase())) ||
     (searchOption === 'Address' && (l.address).toLowerCase().includes(searchText.toLowerCase()))
   );
 }

  compareForSort(first, second)
  {
    let property = '';
    if(this.state.sortOption === 'Name') { property = 'firstName'; }
    if(this.state.sortOption === 'Address') { property = 'address'; }


    if (first[property] === second[property]) return 0;
    if(this.state.sortReversed)
    {
      if (first[property] > second[property]) return -1;
      else return 1;
    } else {
      if (first[property] < second[property]) return -1;
      else return 1;
    }
  }


  componentDidMount() {
    GetLocations(this.updateState);
  }

  render() {
  return (
    <div className="list-container">
      <SearchBar handleChange={this.handleChange} searchOptions={this.state.searchOptions} updateState={this.updateState} />
        <Link className="" to={{ pathname: `/Admin/Locations/0` }}>
          <div>
            <h5>Create new location</h5>
          </div>
        </Link>
      {
        this.getFilteredAndSortedLocations().map(l =><LocationCell location={l} editable={true}/> )
      }
    </div>
  )
  }
 }
