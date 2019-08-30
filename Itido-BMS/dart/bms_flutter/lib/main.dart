import 'package:bms_flutter/src/resources/firestore_cloud_storage_controller.dart';
import 'package:flutter/material.dart';
import 'package:bloc/bloc.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_dart/blocs.dart';
import 'package:flutter_packages/storage.dart';
import 'src/language/translations.dart';
import 'src/resources/flutter_repository_provider.dart';
import 'src/resources/image_controller.dart';
import 'src/resources/test_resources/fake_flutter_repository_provider.dart';

import 'package:flutter_localizations/flutter_localizations.dart';
import 'src/language/application.dart';

class SimpleBlocDelegate extends BlocDelegate {
  @override
  void onEvent(Bloc bloc, Object event) {
    super.onEvent(bloc, event);
    print(event);
  }

  @override
  void onTransition(Bloc bloc, Transition transition) {
    super.onTransition(bloc, transition);
    print(transition);
  }

  @override
  void onError(Bloc bloc, Object error, StackTrace stacktrace) {
    super.onError(bloc, error, stacktrace);
    print(error);
  }
}

void main(Map<String, Widget Function(BuildContext)> routes) async {
  BlocSupervisor.delegate = SimpleBlocDelegate();

  // repositoryProvider = FakeFlutterRepositoryProvider();

  WidgetsFlutterBinding.ensureInitialized();

  await Storage.setPath();

  repositoryProvider = FlutterRepositoryProvider();

  await ImageController.initialize(Storage.localPath);
  await FirestoreCloudStorageController.initialize(Storage.localPath);

  runApp(BlocProviderTree(
    blocProviders: [
      BlocProvider<AuthenticationBloc>(
        builder: (context) {
          return AuthenticationBloc()..dispatch(AppStarted());
        },
      ),
    ],
    child: App(
      routes: routes,
    ),
  ));
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

class App extends StatefulWidget {
  final Map<String, Widget Function(BuildContext)> routes;

  const App({Key key, this.routes}) : super(key: key);
  @override
  _AppState createState() => _AppState();
}

class _AppState extends State<App> {
  BmsLocalizationsDelegate _bmsLocalizationsDelegate;

  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    _bmsLocalizationsDelegate = BmsLocalizationsDelegate(newLocale: null);
    application.onLocaleChanged = onLocaleChange;
  }

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      localizationsDelegates: [
        _bmsLocalizationsDelegate,
        const BmsLocalizationsDelegate(),
        GlobalMaterialLocalizations.delegate,
        GlobalWidgetsLocalizations.delegate,
      ],
      supportedLocales: [
        const Locale('en'), // English
        const Locale('da'), // Danish
      ],
      //home: AuthenticationDecisionScreen(),
      initialRoute: '/',
      routes: widget.routes,
    );
  }

  void onLocaleChange(Locale locale) {
    print('locale change: ${locale.languageCode}');
    setState(() {
      _bmsLocalizationsDelegate = BmsLocalizationsDelegate(newLocale: locale);
    });
  }
}
