class Login {
  String userName;
  String password;

  Login({
    this.userName,
    this.password,
  });

  Map<String, dynamic> toMap() => {
        'userName': this.userName,
        'password': this.password,
      };
}
