import React, { Component } from 'react';
import { UserCell } from '../user/UserCell'
import { GetUsers, UpdateCustomer, CreateCustomer } from '../api/API.js'
import ic_back from '../images/ic_back.svg'

import '../create-style.css';

export class CustomerCreate extends Component {
 constructor(props) {
   super(props);

   let customer = { name: '', comment: '', keyAccountManagerID: null, keyAccountManager: { id: 0, firstName: ''}, mainUserID: null, mainUser: { id: 0, firstName: ''}, };
   let newCustomer = true;

   if(props.location.params !== undefined)
   {
      customer = props.location.params.customer;
      newCustomer = false;
   }

   this.state = {
     newCustomer: newCustomer,
     customer: customer,
     isLoading: false,
     showMainUsers: false,
     showKAMUsers: false,
     users: []
   };

   this.handleChange = this.handleChange.bind(this);
   this.update = this.update.bind(this);
   this.create = this.create.bind(this);
   this.updateState = this.updateState.bind(this);
   this.keyAccountManagerSelected = this.keyAccountManagerSelected.bind(this);
   this.mainUserSelected = this.mainUserSelected.bind(this);
 }

 updateState(obj) { this.setState(obj); }

 keyAccountManagerSelected(user) {
   if(user === null)
   {
     this.state.customer.keyAccountManager = {id: 0, name: ''};
     this.state.customer.keyAccountManagerID = null;
   } else {
     this.state.customer.keyAccountManager = user;
     this.state.customer.keyAccountManagerID = user.id;
   }
   this.setState({
     showSlUsers: false, customer: this.state.customer
   });
 }

 mainUserSelected(user) {
   if(user === null)
   {
     this.state.customer.mainUser = {id: 0, name: ''};
     this.state.customer.mainUserID = null;
   } else {
     this.state.customer.mainUser = user;
     this.state.customer.mainUserID = user.id;
   }
   this.setState({
     showMainUsers: false, customer: this.state.customer
   });
 }

 handleChange(event)
 {
   let name = event.target.name;
   let value = event.target.value;

   if(name === 'showKAMUsers')
   {
     GetUsers(this.updateState);
     this.state.showKAMUsers = true;
   }
   else if(name === 'showMainUsers')
   {
     GetUsers(this.updateState);
     this.state.showMainUsers = true;
   }
   else {
     this.state.customer[name] = value;
   }
   this.setState({
     customer: this.state.customer
   });

 }

 create(event) {
   event.preventDefault();
   CreateCustomer(this.state.customer, this.updateState);
 }

 update(event) {
   event.preventDefault();
   UpdateCustomer(this.state.customer, this.updateState);
 }

 render() {
 return (
   this.state.showKAMUsers ?
   <div className="page-container">
     <div className="flex-container">
       <button className="back-button" onClick={() => this.setState({ showKAMUsers: false})}>
         <img className="back-icon" src={ic_back} />
       </button>
       <h3>Select a key account manager</h3>
     </div>
     {
       this.state.users.map(u =>
        <div key={u.id} onClick={() => this.keyAccountManagerSelected(u) } >
          <UserCell user={u} />
        </div>
        )
      }
   </div>
   : this.state.showMainUsers ?
   <div className="page-container">
     <div className="flex-container">
       <button className="back-button" onClick={() => this.setState({ showMainUsers: false})}>
         <img className="back-icon" src={ic_back} />
       </button>
       <h3>Select a customer manager</h3>
     </div>
     {
       this.state.users.map(u =>
        <div key={u.id} onClick={() => this.mainUserSelected(u) } >
          <UserCell user={u} />
        </div>
        )
      }
   </div>
    : <form className="row page-container" onSubmit={ this.state.newCustomer ? this.create : this.update}>
      <h2 className="col-12">New customer</h2>
      <div className="col-12 row">
        <h4 className="col-12 col-md-4">Name</h4>
        <input name="name" className="col-12 col-md-8" type="text" required value={this.state.customer.name} onChange={this.handleChange}/>
      </div>
      <div className="col-12 row">
        <h4 className="col-12 col-md-4">Comment</h4>
        <textarea name="comment" className="col-12 col-md-8" type="text" value={this.state.customer.comment} onChange={this.handleChange}/>
      </div>

      <div className="col-12 row">
        <h4 className="col-12 col-md-4">Key account manager</h4>
        <input name="showKAMUsers" className="col-12 col-md-8" required type="button" value={this.state.customer.keyAccountManager.firstName} onClick={this.handleChange}/>
      </div>
      <div className="col-12 row">
        <h4 className="col-12 col-md-4">Customer manager</h4>
        <input name="showMainUsers" className="col-12 col-md-8" required type="button" value={this.state.customer.mainUser.firstName} onClick={this.handleChange}/>
      </div>

      <div className="col-12 row">
        <div className="col-12 col-md-4"></div>
        <div className="submit-div col-12 col-md-8">
          <button className="submit-button" type="submit">Create customer</button>
        </div>
      </div>
   </form>
 )
 }
}
