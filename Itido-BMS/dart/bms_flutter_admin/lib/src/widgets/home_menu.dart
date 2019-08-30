import 'package:bms_dart/absence_reason_list_bloc.dart';
import 'package:bms_dart/agreement_list_bloc.dart';
// import 'package:bms_dart/post_list_bloc.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_dart/blocs.dart';
import 'package:bms_flutter/translations.dart';

// import 'package:bms_flutter_admin/src/screens/customer_inspect_screen.dart';
// import 'package:bms_flutter_admin/src/screens/location_inspect_screen.dart';
import 'package:bms_flutter_admin/src/screens/user_inspect_screen.dart';

import 'package:bms_flutter/src/widgets/user/widgets.dart';
import 'package:bms_flutter/src/widgets/agreement/widgets.dart';
import 'package:bms_flutter/src/widgets/absence_reason/widgets.dart';
// import 'package:bms_flutter/src/widgets/post/widgets.dart';

class HomeMenu extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    var _authenticationBloc = BlocProvider.of<AuthenticationBloc>(context);

    return Column(
      crossAxisAlignment: CrossAxisAlignment.stretch,
      children: <Widget>[
        Container(
          color: Colors.grey[200],
          child: SafeArea(
            left: false,
            right: false,
            bottom: false,
            child: Container(
              height: 160,
              child: Center(
                child: Text(''),
              ),
            ),
          ),
        ),
        Expanded(
          child: ListView(
            children: <Widget>[
              ListTile(
                title: Text(Translations.of(context).buttonUsers),
                leading: Icon(Icons.group),
                onTap: () {
                  UserListScreen.show(
                    context,
                    blocBuilder: (context) => UserListBloc(() => FetchAll()),
                    onSelect: (user) =>
                        UserInspectScreen.show(context, user.id),
                    floatingActionButton: FloatingActionButton(
                      child: Icon(Icons.add),
                      onPressed: () {
                        Navigator.of(context).push(
                          MaterialPageRoute(
                            builder: (context) => UserCreateUpdateScreen(),
                          ),
                        );
                      },
                    ),
                  );
                },
              ),
              // ListTile(
              //   title: Text(Translations.of(context).buttonClients),
              //   leading: Icon(Icons.business),
              //   onTap: () {
              //     CustomerListScreen.show(
              //       context,
              //       blocBuilder: (context) =>
              //           CustomerListBloc(() => CustomerListFetch()),
              //       onSelect: (customer) =>
              //           CustomerInspectScreen.show(context, customer.id),
              //       floatingActionButton: FloatingActionButton(
              //         child: Icon(Icons.add),
              //         onPressed: () {
              //           Navigator.of(context).push(
              //             MaterialPageRoute(
              //               builder: (context) => CustomerCreateUpdateScreen(),
              //             ),
              //           );
              //         },
              //       ),
              //     );
              //   },
              // ),
              // ListTile(
              //   title: Text(Translations.of(context).buttonLocations),
              //   leading: Icon(Icons.location_on),
              //   onTap: () {
              //     LocationListScreen.show(
              //       context,
              //       blocBuilder: (context) =>
              //           LocationListBloc(() => LocationListFetchAll()),
              //       onSelect: (location) =>
              //           LocationInspectScreen.show(context, location.id),
              //     );
              //   },
              // ),
              ListTile(
                title: Text(Translations.of(context).buttonAgreements),
                leading: Icon(Icons.assignment_ind),
                onTap: () {
                  AgreementListScreen.show(
                    context,
                    blocBuilder: (context) =>
                        AgreementListBloc(() => AgreementListFetch()),
                    onSelect: (agreement) => AgreementCreateUpdateScreen.show(
                      context,
                      agreement: agreement,
                    ),
                    floatingActionButton: FloatingActionButton(
                      child: Icon(Icons.add),
                      onPressed: () {
                        AgreementCreateUpdateScreen.show(context);
                      },
                    ),
                  );
                },
              ),
              ListTile(
                title: Text(Translations.of(context).buttonAbsenseReasons),
                leading: Icon(Icons.sentiment_dissatisfied),
                onTap: () {
                  AbsenceReasonListScreen.show(
                    context,
                    blocBuilder: (context) =>
                        AbsenceReasonListBloc(() => AbsenceReasonListFetch()),
                    onSelect: (absenceReason) =>
                        AbsenceReasonCreateUpdateScreen.show(
                      context,
                      absenceReason: absenceReason,
                    ),
                    floatingActionButton: FloatingActionButton(
                      child: Icon(Icons.add),
                      onPressed: () {
                        AbsenceReasonCreateUpdateScreen.show(context);
                      },
                    ),
                  );

                  // Navigator.of(context)
                  //     .pushNamed('/absence_reason_list_all_screen');
                },
              ),
              ListTile(
                title: Text(Translations.of(context).buttonConversations),
                leading: Icon(Icons.chat),
                onTap: () {
                  Navigator.of(context).pushNamed('/conversation_list_screen');
                },
              ),
              // ListTile(
              //   title: Text(Translations.of(context).buttonPosts),
              //   leading: Icon(Icons.markunread_mailbox),
              //   onTap: () {
              //     PostListScreen.show(
              //       context,
              //       blocBuilder: (context) =>
              //           PostListBloc(() => PostListFetch(more: false)),
              //       floatingActionButton: FloatingActionButton(
              //         child: Icon(Icons.add),
              //         onPressed: () {
              //           PostCreateUpdateScreen.show(context);
              //         },
              //       ),
              //     );

              //     // Navigator.of(context).pushNamed('/post_list_all_screen');
              //   },
              // ),
              // ListTile(
              //   title: Text(Translations.of(context).buttonAccidentReports),
              //   leading: Icon(Icons.warning),
              //   onTap: () {
              //     Navigator.of(context)
              //         .pushNamed('/accident_report_list_all_screen');
              //   },
              // ),
              ListTile(
                title: Text('English'),
                onTap: () {
                  application.onLocaleChanged(Locale('en'));
                },
              ),
              ListTile(
                title: Text('Dansk'),
                onTap: () {
                  application.onLocaleChanged(Locale('da'));
                },
              ),
            ],
          ),
        ),
        Container(
          child: SafeArea(
            left: false,
            right: false,
            top: false,
            child: Container(
              height: 60,
              child: RaisedButton(
                child: Text(Translations.of(context).buttonLogout),
                onPressed: () {
                  _authenticationBloc.dispatch(LoggedOut());
                },
              ),
            ),
          ),
        ),
      ],
    );
  }
}
