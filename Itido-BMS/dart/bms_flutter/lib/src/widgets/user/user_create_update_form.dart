import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/user_create_update_bloc.dart';
import 'package:bms_flutter/components.dart';
import 'package:bms_flutter/src/components/check_box_row.dart';
import 'package:bms_flutter/src/components/decorated_drop_down_button.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class UserCreateUpdateForm extends StatefulWidget {
  @override
  _UserCreateUpdateFormState createState() => _UserCreateUpdateFormState();
}

class _UserCreateUpdateFormState extends State<UserCreateUpdateForm> {
  TextEditingController _userNameController;
  TextEditingController _passwordController;
  TextEditingController _firstNameController;
  TextEditingController _lastNameController;
  TextEditingController _emailController;
  TextEditingController _phoneController;
  TextEditingController _commentController;
  TextEditingController _employeeNumberController;
  TextEditingController _languageCodeController;

  @override
  Widget build(BuildContext context) {
    var bloc = BlocProvider.of<UserCreateUpdateBloc>(context);

    return BlocListener(
      bloc: bloc,
      listener: (context, UserCreateUpdateState state) {
        if (state.createUpdateStatePhase == CreateUpdateStatePhase.Initial) {
          _userNameController = (_userNameController ?? TextEditingController())
            ..text = state.login.userName;
          _passwordController = (_passwordController ?? TextEditingController())
            ..text = state.login.password;
          _firstNameController = (_firstNameController ??
              TextEditingController())
            ..text = state.user.firstName;
          _lastNameController = (_lastNameController ?? TextEditingController())
            ..text = state.user.lastName;
          _emailController = (_emailController ?? TextEditingController())
            ..text = state.user.email;
          _phoneController = (_phoneController ?? TextEditingController())
            ..text = state.user.phone;
          _commentController = (_commentController ?? TextEditingController())
            ..text = state.user.comment;
          _languageCodeController = (_languageCodeController ??
              TextEditingController())
            ..text = state.user.languageCode;
          _employeeNumberController = (_employeeNumberController ??
              TextEditingController())
            ..text = (state.user.employeeNumber ?? '').toString();
        }
      },
      child: SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.all(24.0),
          child: BlocBuilder(
            bloc: bloc,
            builder: (context, UserCreateUpdateState state) {
              if (state.createUpdateStatePhase ==
                  CreateUpdateStatePhase.Loading)
                return Container(
                  child: Center(
                    child: CircularProgressIndicator(),
                  ),
                );

              return Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: <Widget>[
                  CircleAvatar(
                    radius: 60,
                  ),
                  Space(height: 8),
                  Center(child: Text('Billede')),
                  Space(height: 24),
                  CheckBoxRow(
                    title: 'Er brugeren knyttet til en klient?',
                    value: state.isClientUser,
                    onChanged: (isOn) =>
                        bloc.dispatch(IsClientChanged(isOn: isOn)),
                  ),
                  Space(),
                  DecoratedDropDownButton<int>(
                    onChanged: (projectRoleId) => bloc.dispatch(
                        ProjectRoleChanged(
                            projectRole: state.projectRoles.firstWhere(
                                (pr) => pr.id == projectRoleId,
                                orElse: () => null))),
                    labelText: 'Projekt-rolle',
                    hintText: 'Brugeres projekt rettigheder',
                    allValues: state.projectRoles.map((r) => r.id).toList(),
                    selectedValue: state.selectedProjectRole?.id,
                    valueToString: (id) =>
                        state.projectRoles
                            .firstWhere((pr) => pr.id == id, orElse: () => null)
                            ?.name ??
                        '??',
                  ),
                  Space(),
                  if (!state.isClientUser)
                    DecoratedDropDownButton<String>(
                      onChanged: (userRole) =>
                          bloc.dispatch(UserRoleChanged(text: userRole)),
                      labelText: 'Bruger-rolle',
                      hintText: 'Brugeres bruger rettigheder',
                      allValues: state.userRoles,
                      selectedValue: state.selectedUserRole,
                      valueToString: (role) => role,
                    ),
                  if (state.isClientUser)
                    DecoratedDropDownButton<int>(
                      onChanged: (clientId) => bloc.dispatch(ClientChanged(
                          client: state.clients.firstWhere(
                              (pr) => pr.id == clientId,
                              orElse: () => null))),
                      labelText: 'Klinet',
                      hintText: 'Brugeres klient',
                      allValues: state.clients.map((r) => r.id).toList(),
                      selectedValue: state.selectedClient?.id,
                      valueToString: (id) =>
                          state.clients
                              .firstWhere((pr) => pr.id == id,
                                  orElse: () => null)
                              ?.name ??
                          '??',
                    ),
                  Space(height: 40),
                  TextField(
                    decoration:
                        InputDecoration(labelText: 'FirstName', filled: true),
                    controller: _firstNameController,
                    onChanged: (text) =>
                        bloc.dispatch(FirstNameChanged(text: text)),
                  ),
                  Space(),
                  TextField(
                    decoration:
                        InputDecoration(labelText: 'LastName', filled: true),
                    controller: _lastNameController,
                    onChanged: (text) =>
                        bloc.dispatch(LastNameChanged(text: text)),
                  ),
                  if (!state.isClientUser)
                    Padding(
                      padding: EdgeInsets.only(top: 40, bottom: 16),
                      child: Text(
                        'Kontaktoplysninger',
                        style: Theme.of(context).textTheme.title,
                      ),
                    ),
                  if (!state.isClientUser)
                    TextField(
                      decoration: InputDecoration(
                        labelText: 'Email',
                        filled: true,
                        errorText:
                            state.errors.emailValid ? null : 'Email incorrect',
                      ),
                      controller: _emailController,
                      onChanged: (text) =>
                          bloc.dispatch(EmailChanged(text: text)),
                      keyboardType: TextInputType.emailAddress,
                    ),
                  if (!state.isClientUser)
                    Space(),
                  if (!state.isClientUser)
                    TextField(
                      decoration:
                          InputDecoration(labelText: 'Phone', filled: true),
                      controller: _phoneController,
                      onChanged: (text) =>
                          bloc.dispatch(PhoneChanged(text: text)),
                      keyboardType: TextInputType.phone,
                    ),
                  if (!state.isClientUser)
                    Padding(
                      padding: EdgeInsets.only(top: 40, bottom: 16),
                      child: Text(
                        'Andre personale-oplysninger',
                        style: Theme.of(context).textTheme.title,
                      ),
                    ),

                  if (!state.isClientUser)
                    TextField(
                      decoration: InputDecoration(
                          labelText: 'EmployeeNumber', filled: true),
                      controller: _employeeNumberController,
                      onChanged: (text) =>
                          bloc.dispatch(EmployeeNumberChanged(text: text)),
                      keyboardType: TextInputType.number,
                    ),
                  if (!state.isClientUser)
                    Space(),
                  if (!state.isClientUser)
                    TextField(
                      decoration: InputDecoration(
                          labelText: 'LanguageCode', filled: true),
                      controller: _languageCodeController,
                      onChanged: (text) =>
                          bloc.dispatch(LanguageCodeChanged(text: text)),
                    ),
                  // Space(),
                  // TextField(
                  //   maxLines: 10,
                  //   decoration:
                  //       InputDecoration(labelText: 'Comment', filled: true),
                  //   controller: _commentController,
                  //   onChanged: (text) =>
                  //       bloc.dispatch(CommentChanged(text: text)),
                  // ),
                  Padding(
                    padding: EdgeInsets.only(top: 40, bottom: 16),
                    child: Text(
                      'Loginoplysninger',
                      style: Theme.of(context).textTheme.title,
                    ),
                  ),
                  if (state.isCreate)
                    TextField(
                      decoration: InputDecoration(
                        labelText: 'Username',
                        filled: true,
                        errorText: state.errors.usernameValid
                            ? null
                            : 'Username too short',
                      ),
                      controller: _userNameController,
                      onChanged: (text) =>
                          bloc.dispatch(UserNameChanged(text: text)),
                    ),
                  if (state.isCreate)
                    Space(),
                  if (state.isCreate)
                    TextField(
                      decoration: InputDecoration(
                        labelText: 'Password',
                        filled: true,
                        errorText: state.errors.passwordValid
                            ? null
                            : 'Password too short',
                      ),
                      controller: _passwordController,
                      onChanged: (text) =>
                          bloc.dispatch(PasswordChanged(text: text)),
                      obscureText: true,
                    ),
                  Space(height: 40),
                  Center(
                    child: RaisedButton(
                      child: Text('SUBMIT'),
                      onPressed: state.isValid
                          ? () {
                              bloc.dispatch(Commit());
                            }
                          : null,
                    ),
                  ),
                  Space(height: 40),
                ],
              );
            },
          ),
        ),
      ),
    );
  }
}
