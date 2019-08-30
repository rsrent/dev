let Config = require('../../config.json');

function getHeaders()
{
  let myHeaders = new Headers();
  myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
  myHeaders.append("Content-Type", "application/json");
  return myHeaders;
}

export function GetUsers(updateState)
{
  updateState({ isLoading: true });
  fetch(Config.API_URL + "/Admin/Users", {
      method: 'GET',
      headers: getHeaders()
    })
    .then((response) => {
      console.log(response);
      return response.json();
    })
    .then(json => {
      console.log(json);
      updateState({ isLoading: false, users: json });
    })
    .catch(error => {
        console.log(error);
        alert("Der skete en ukendt serverfejl 2");
    })
    .finally(() => {

    });
}

export function UpdateUser(user, updateState)
{
  console.log("UpdateUser");
  console.log(user);

  updateState({ isLoading: true });
  fetch(Config.API_URL + "/Admin/Users", {
      method: 'PUT',
      body: JSON.stringify(user),
      headers: getHeaders()
    })
    .then(response => response.json() )
    .then(json => {
      updateState({ user: json });
    })
    .catch(error => {
        alert("Der skete en ukendt serverfejl 3");
    });
}

export function CreateUser(user, updateState)
{
  console.log("CreateUser");
  console.log(user);

  updateState({ isLoading: true });
  fetch(Config.API_URL + "/Admin/Users", {
      method: 'POST',
      body: JSON.stringify(user),
      headers: getHeaders()
    })
    .then(response => response.json() )
    .then(json => {
      updateState({ user: json });
    })
    .catch(error => {
        alert("Der skete en ukendt serverfejl 4");
    });
}




//Locations
export function GetLocations(updateState)
{
  updateState({ isLoading: true });
  fetch(Config.API_URL + "/Admin/Locations", {
      method: 'GET',
      headers: getHeaders(),
    })
    .then((response) => {
      return response.json();
    })
    .then(json => {
      updateState({ isLoading: false, locations: json });
    })
    .catch(error => {
        alert("Der skete en ukendt serverfejl 5");
    })
    .finally(() => {

    });
}

export function UpdateLocation(location, updateState)
{
  console.log("UpdateLocation");
  console.log(location);

  updateState({ isLoading: true });
  fetch(Config.API_URL + "/Admin/Locations", {
      method: 'PUT',
      body: JSON.stringify(location),
      headers: getHeaders()
    })
    .then(response => response.json() )
    .then(json => {
      updateState({ location: json });
    })
    .catch(error => {
        alert("Der skete en ukendt serverfejl 6");
    });
}

export function CreateLocation(location, updateState)
{
  console.log("CreateLocation");
  console.log(location);

  updateState({ isLoading: true });
  fetch(Config.API_URL + "/Admin/Locations", {
      method: 'POST',
      body: JSON.stringify(location),
      headers: getHeaders()
    })
    .then(response => response.json() )
    .then(json => {
      updateState({ location: json });
    })
    .catch(error => {
        alert("Der skete en ukendt serverfejl 7");
    });
}


//Customers
export function GetCustomers(updateState)
{
  updateState({ isLoading: true });
  fetch(Config.API_URL + "/Admin/Customers", {
      method: 'GET',
      headers: getHeaders()
    })
    .then((response) => {
      console.log(response);
      return response.json();
    })
    .then(json => {
      console.log(json);
      updateState({ isLoading: false, customers: json });
    })
    .catch(error => {
        console.log(error);
        alert("Der skete en ukendt serverfejl 8");
    })
    .finally(() => {

    });
}

export function UpdateCustomer(customer, updateState)
{
  console.log("UpdateCustomer");
  console.log(customer);
  updateState({ isLoading: true });
  fetch(Config.API_URL + "/Admin/Customers", {
      method: 'PUT',
      body: JSON.stringify(customer),
      headers: getHeaders()
    })
    .then(response => response.json() )
    .then(json => {
      updateState({ customer: json });
    })
    .catch(error => {
        alert("Der skete en ukendt serverfejl 9");
    });
}

export function CreateCustomer(customer, updateState)
{
  console.log("CreateCustomer");
  console.log(customer);

  updateState({ isLoading: true });
  fetch(Config.API_URL + "/Admin/Customers", {
      method: 'POST',
      body: JSON.stringify(customer),
      headers: getHeaders()
    })
    .then(response => response.json() )
    .then(json => {
      updateState({ customer: json });
    })
    .catch(error => {
        alert("Der skete en ukendt serverfejl 10");
    });
}



//roles
export function GetRoles(updateState)
{
  updateState({ isLoading: true });
  fetch(Config.API_URL + "/Admin/Roles", {
    method: 'GET',
    headers: getHeaders()
  })
    .then(response => response.json() )
    .then(json => {
      updateState({ roles: json });
    })
    .catch(error => {
        alert("Der skete en ukendt serverfejl 11");
    })
    .finally(() => {

    });
}
