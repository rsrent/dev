import React, { Component } from 'react';
import { CustomerCell } from '../customer/CustomerCell'
import { GetCustomers, GetRoles, UpdateUser, CreateUser } from '../api/API.js'
import ic_back from '../images/ic_back.svg'

import '../create-style.css';

export class UserCreate extends Component {
 constructor(props) {
   super(props);

   let user = { email: '', phone: '', firstName: '', lastName: '', comment: '',
                imageLocation: '', customerID: null, employeeNumber: '', title: '', role: { id: 0}, customer: {id: 0, name: ''}};
   let newUser = true;
   let isCustomerUser = false;
   if(props.location.params !== undefined)
   {
      user = props.location.params.user;
      newUser = false;
      console.log(user.customer)
      if(user.customer !== null)
      {
        isCustomerUser = true;
      }
   }

   this.state = {
     newUser: newUser,
     user: user,
     roles: [],
     isLoading: false,
     isCustomerUser: isCustomerUser,
     showCustomers: false,
     customers: []
   };

   this.handleChange = this.handleChange.bind(this);
   this.update = this.update.bind(this);
   this.create = this.create.bind(this);
   this.updateState = this.updateState.bind(this);
   this.customerSelected = this.customerSelected.bind(this);
 }

 updateState(obj) { this.setState(obj); }

 customerSelected(customer) {
   if(customer === null)
   {
     this.state.user.customer = {id: 0, name: ''};
     this.state.user.customerID = null;
   } else {
     this.state.user.customer = customer;
     this.state.user.customerID = customer.id;
   }
   this.setState({
     showCustomers: false, user: this.state.user
   });
 }

 componentDidMount() {
   GetRoles(this.updateState);
 }

 handleChange(event)
 {
   let name = event.target.name;
   let value = event.target.value;

   if(name === 'showCustomer')
   {
     GetCustomers(this.updateState);
     this.state.showCustomers = true;
   }
   else if(name === 'role')
   {
     this.state.user.role = this.state.roles.find(r => r.id === parseInt(value));
     if(this.state.user.roleId === 8 || this.state.user.roleId === 9)
     {
       this.setState({ isCustomerUser: true });
     } else {
       this.setState({ isCustomerUser: false });
       this.state.user.customer = {id: 0, name: ''};
       this.setState({
         user: this.state.user
       });
     }
   } else {
     this.state.user[name] = value;
   }
   this.setState({
     user: this.state.user
   });

 }

 create(event) {
   event.preventDefault();
   CreateUser(this.state.user, this.updateState);
 }

 update(event) {
   event.preventDefault();
   UpdateUser(this.state.user, this.updateState);
 }

 render() {
 return (
   this.state.showCustomers ?
   <div className="page-container">
     <div className="flex-container">
       <button className="back-button" onClick={() => this.setState({ showCustomers: false})}>
         <img className="back-icon" src={ic_back} />
       </button>
       <h3>Select a Customer</h3>
     </div>
     {
       this.state.customers.map(c =>
        <div key={c.id} onClick={() => this.customerSelected(c) } >
          <CustomerCell customer={c} />
        </div>
        )
      }
   </div>

    : <form className="row page-container" onSubmit={ this.state.newUser ? this.create : this.update}>
      <h2 className="col-12">New user</h2>
      <div className="col-12 row">
        <h4 className="col-12 col-md-4" >First name</h4>
        <input name="firstName" className="col-12 col-md-8" type="text" required value={this.state.user.firstName} onChange={this.handleChange}/>
      </div>
      <div className="col-12 row">
        <h4 className="col-12 col-md-4" >Last name</h4>
        <input name="lastName" className="col-12 col-md-8" type="text" required value={this.state.user.lastName} onChange={this.handleChange}/>
      </div>
      <div className="col-12 row">
        <h4 className="col-12 col-md-4" >Email</h4>
        <input name="email" className="col-12 col-md-8" type="email" required value={this.state.user.email} onChange={this.handleChange}/>
      </div>
      <div className="col-12 row">
        <h4 className="col-12 col-md-4" >Phone</h4>
        <input name="phone" className="col-12 col-md-8" type="tel" required value={this.state.user.phone} onChange={this.handleChange}/>
      </div>
      <div className="col-12 row">
        <h4 className="col-12 col-md-4" >Comment</h4>
        <textarea name="comment" className="col-12 col-md-8" type="text" value={this.state.user.comment} onChange={this.handleChange}/>
      </div>
      <div className="col-12 row">
        <h4 className="col-12 col-md-4" >User number</h4>
        <input name="employeeNumber" className="col-12 col-md-8" type="text" value={this.state.user.employeeNumber} onChange={this.handleChange}/>
      </div>
      <div className="col-12 row">
        <h4 className="col-12 col-md-4" >Role</h4>
        <select name="role" className="col-12 col-md-8" value={this.state.user.role.id} onChange={this.handleChange}>
          <option value="">Select a role</option>
          { this.state.roles.map((role) => { return <option value={role.id}>{role.name}</option> })}
        </select>
      </div>
      {
        this.state.isCustomerUser ? <div className="col-12 row">
          <h4 className="col-12 col-md-4" >Customer</h4>
          <input name="showCustomer" className="col-12 col-md-8" required type="button" value={this.state.user.customer.name} onClick={this.handleChange}/>
        </div> : <div></div>
      }
      <div className="col-12 row">
        <div className="col-12 col-md-4"></div>
        <div className="submit-div col-12 col-md-8">
          <button className="submit-button" type="submit">Create user</button>
        </div>
      </div>
   </form>
 )
 }
}
