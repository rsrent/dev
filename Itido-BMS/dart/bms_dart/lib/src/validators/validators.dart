bool validateUsername(String name) => name != null && name.length >= 4;
bool validatePassword(String password) =>
    password != null && password.length >= 6;
bool validateEmail(String email) =>
    email != null &&
    //RegExp(r"^[a-zA-Z0-9.]+@[a-zA-Z0-9]+\.[a-zA-Z]+").hasMatch(email);
    RegExp(r"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$")
        .hasMatch(email);
