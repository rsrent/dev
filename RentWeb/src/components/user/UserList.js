import React, { Component } from 'react';
import {Link} from "react-router-dom";
import { SearchBar } from '../search/SearchBar'
import { UserCell } from './UserCell'
import { GetUsers } from '../api/API.js'
import '../list-style.css';
export class UserList extends Component {
 constructor(props) {
   super(props);

   this.state = {
     searchText: '',
     searchOptions: ['Name', 'Title', 'Phone', 'Email'],
     searchOption: 'Name',
     sortOption: 'Name',
     sortReversed: false,
     isLoading: false,
     users: []
   }

   this.getFilteredAndSortedUsers = this.getFilteredAndSortedUsers.bind(this);
   this.compareForSort = this.compareForSort.bind(this);
   this.updateState = this.updateState.bind(this);
 }

 updateState(obj)
 {
   this.setState(obj);
 }

 getFilteredAndSortedUsers() {
   let users = this.state.users.sort(this.compareForSort);

   let searchOption = this.state.searchOption;
   let searchText = this.state.searchText;
   return users.filter(u =>
     (searchOption === 'Name' && (u.firstName + ' ' + u.lastName).toLowerCase().includes(searchText.toLowerCase())) ||
     (searchOption === 'Phone' && (u.phone).toLowerCase().includes(searchText.toLowerCase())) ||
     (searchOption === 'Email' && (u.email).toLowerCase().includes(searchText.toLowerCase())) ||
     (searchOption === 'Role' && (u.role.name).toLowerCase().includes(searchText.toLowerCase())) ||
     (searchOption === 'Title' && (u.title).toLowerCase().includes(searchText.toLowerCase()))
   );
 }

  compareForSort(first, second)
  {
    let property = '';
    if(this.state.sortOption === 'Name') { property = 'firstName'; }
    if(this.state.sortOption === 'Title') { property = 'title'; }
    if(this.state.sortOption === 'Phone') { property = 'phone'; }
    if(this.state.sortOption === 'Email') { property = 'email'; }

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
    GetUsers(this.updateState);
  }

 render() {
 return (
   <div className="list-container">
     <SearchBar searchOptions={this.state.searchOptions} sortReversed={this.state.sortReversed} updateState={this.updateState}  />
       <Link className="" to={{ pathname: `/Admin/Users/0` }}>
         <div>
           <h5>Create new user</h5>
         </div>
       </Link>
     {
       this.state.isLoading ? <p>loading</p> : this.getFilteredAndSortedUsers().map(u => <UserCell user={u} editable={true} /> )
     }
   </div>
 )
 }
}
