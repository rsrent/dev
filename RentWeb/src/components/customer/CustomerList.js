import React, { Component } from 'react';
import {Link} from "react-router-dom";
import { SearchBar } from '../search/SearchBar'
import { CustomerCell } from './CustomerCell'
import { GetCustomers } from '../api/API.js'
import '../list-style.css';

export class CustomerList extends Component {
 constructor(props) {
   super(props);

   this.state = {
     searchText: '',
     searchOptions: ['Name'],
     searchOption: 'Name',
     sortOption: 'Name',
     sortReversed: false,
     isLoading: false,
     customers: [],
   }

   this.getFilteredAndSortedCustomers = this.getFilteredAndSortedCustomers.bind(this);
   this.compareForSort = this.compareForSort.bind(this);
   this.updateState = this.updateState.bind(this);
 }

 updateState(obj)
 {
   this.setState(obj);
 }

 getFilteredAndSortedCustomers() {
   let customers = this.state.customers;//.sort(this.compareForSort);

   let searchOption = this.state.searchOption;
   let searchText = this.state.searchText;
   return customers.filter(l =>
     (searchOption === 'Name' && (l.name).toLowerCase().includes(searchText.toLowerCase()))
   );
 }

  compareForSort(first, second)
  {
    let property = '';
    if(this.state.sortOption === 'Name') { property = 'firstName'; }

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
    GetCustomers(this.updateState);
  }

  render() {
  return (
    <div className="list-container">

      <SearchBar handleChange={this.handleChange} searchOptions={this.state.searchOptions} />
        <Link className="" to={{ pathname: `/Admin/Customers/0` }}>
          <div>
            <h5>Create new customer</h5>
          </div>
        </Link>
      {
        this.state.isLoading ? <p>loading</p> : this.getFilteredAndSortedCustomers().map(c => <CustomerCell customer={c} editable={true} /> )
      }
    </div>
  )
  }
 }
