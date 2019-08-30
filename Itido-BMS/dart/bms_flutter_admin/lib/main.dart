// import 'package:bms_flutter_admin/src/widgets/customer_list_admin.dart';
// import 'package:bms_flutter_admin/src/widgets/user_list_admin.dart';
import 'package:flutter/material.dart';

import 'src/screens/conversation_list_screen.dart';
// import 'src/screens/agreement_list_all_screen.dart';
// import 'src/screens/absence_reason_list_all_screen.dart';
// import 'src/screens/post_list_all_screen.dart';
import 'package:bms_flutter/src/screens/authentication_decision_screen.dart';
import 'package:bms_flutter/src/widgets/login/widgets.dart';
import 'src/screens/splash_screen.dart';
import 'src/screens/home_screen.dart';
import 'package:bms_flutter/main.dart' as flutter_app;

// import 'src/widgets/location_list_admin.dart';

// class SimpleBlocDelegate extends BlocDelegate {
//   @override
//   void onEvent(Bloc bloc, Object event) {
//     super.onEvent(bloc, event);
//     print(event);
//   }

//   @override
//   void onTransition(Bloc bloc, Transition transition) {
//     super.onTransition(bloc, transition);
//     print(transition);
//   }

//   @override
//   void onError(Bloc bloc, Object error, StackTrace stacktrace) {
//     super.onError(bloc, error, stacktrace);
//     print(error);
//   }
// }

void main() {
  flutter_app.main({
    '/': (BuildContext context) => AuthenticationDecisionScreen(
          splashScreen: (context) => SplashScreen(),
          homeScreen: (context) => HomeScreen(),
          loginScreen: (context) => LoginScreen(),
        ),
    '/login_screen': (BuildContext context) => LoginScreen(),
    '/splash_screen': (BuildContext context) => SplashScreen(),
    '/home_screen': (BuildContext context) => HomeScreen(),
    // '/user_list_all_screen': (BuildContext context) =>
    //     UserListAdmin.getScreenOfAll(context),
    // UserListScreen.getUserListAll(context),
    // '/customer_list_all_screen': (BuildContext context) =>
    //     CustomerListAdmin.getScreenOfAll(context),
    // '/location_list_all_screen': (BuildContext context) =>
    //     LocationListAllScreen(),
    // '/location_list_all_screen': (BuildContext context) =>
    //     LocationListAdmin.getScreenOfAll(context),
    // '/agreement_list_all_screen': (BuildContext context) =>
    //     AgreementListAllScreen(),
    // '/absence_reason_list_all_screen': (BuildContext context) =>
    //     AbsenceReasonListAllScreen(),
    '/conversation_list_screen': (BuildContext context) =>
        ConversationListScreen(),
    // '/post_list_all_screen': (BuildContext context) => PostListAllScreen(),
  });

/*
  BlocSupervisor.delegate = SimpleBlocDelegate();

  // repositoryProvider = FlutterRepositoryProvider();
  repositoryProvider = FakeFlutterRepositoryProvider();

  runApp(BlocProviderTree(
    blocProviders: [
      BlocProvider<AuthenticationBloc>(
        builder: (context) {
          return AuthenticationBloc()..dispatch(AppStarted());
        },
      ),
    ],
    child: App(),
  ));
  */
}

// class App extends StatelessWidget {
//   App({Key key}) : super(key: key);
//   @override
//   Widget build(BuildContext context) {
//     return MaterialApp(
//       localizationsDelegates: [
//         const BmsLocalizationsDelegate(),
//         GlobalMaterialLocalizations.delegate,
//         GlobalWidgetsLocalizations.delegate,
//       ],
//       supportedLocales: [
//         const Locale('en'), // English
//         const Locale('da'), // Danish
//       ],
//       //home: AuthenticationDecisionScreen(),
//       initialRoute: '/',
//       routes: {
//         '/': (BuildContext context) => AuthenticationDecisionScreen(),
//         '/login_screen': (BuildContext context) => LoginScreen(),
//         '/splash_screen': (BuildContext context) => SplashScreen(),
//         '/home_screen': (BuildContext context) => HomeScreen(),
//         '/user_list_all_screen': (BuildContext context) => UserListAllScreen(),
//         '/agreement_list_all_screen': (BuildContext context) =>
//             AgreementListAllScreen(),
//         '/absence_reason_list_all_screen': (BuildContext context) =>
//             AbsenceReasonListAllScreen(),
//         '/conversation_list_screen': (BuildContext context) =>
//             ConversationListScreen(),
//       },
//     );
//   }
// }

// class App extends StatefulWidget {
//   @override
//   _AppState createState() => _AppState();
// }

// class _AppState extends State<App> {
//   BmsLocalizationsDelegate _bmsLocalizationsDelegate;

//   @override
//   void initState() {
//     // TODO: implement initState
//     super.initState();
//     _bmsLocalizationsDelegate = BmsLocalizationsDelegate(newLocale: null);
//     application.onLocaleChanged = onLocaleChange;
//   }

//   @override
//   Widget build(BuildContext context) {
//     return MaterialApp(
//       localizationsDelegates: [
//         _bmsLocalizationsDelegate,
//         const BmsLocalizationsDelegate(),
//         GlobalMaterialLocalizations.delegate,
//         GlobalWidgetsLocalizations.delegate,
//       ],
//       supportedLocales: [
//         const Locale('en'), // English
//         const Locale('da'), // Danish
//       ],
//       //home: AuthenticationDecisionScreen(),
//       initialRoute: '/',
//       routes: {
//         '/': (BuildContext context) => AuthenticationDecisionScreen(),
//         '/login_screen': (BuildContext context) => LoginScreen(),
//         '/splash_screen': (BuildContext context) => SplashScreen(),
//         '/home_screen': (BuildContext context) => HomeScreen(),
//         '/user_list_all_screen': (BuildContext context) => UserListAllScreen(),
//         '/agreement_list_all_screen': (BuildContext context) =>
//             AgreementListAllScreen(),
//         '/absence_reason_list_all_screen': (BuildContext context) =>
//             AbsenceReasonListAllScreen(),
//         '/conversation_list_screen': (BuildContext context) =>
//             ConversationListScreen(),
//       },
//     );
//   }

//   void onLocaleChange(Locale locale) {
//     print('locale change: ${locale.languageCode}');
//     setState(() {
//       _bmsLocalizationsDelegate = BmsLocalizationsDelegate(newLocale: locale);
//     });
//   }
// }
