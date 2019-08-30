import 'package:flutter/material.dart';
import 'blocs/login_provider.dart';
import 'blocs/customer_provider.dart';
import 'blocs/location_provider.dart';
import 'blocs/data_provider.dart';
import 'blocs/task_provider.dart';
import 'blocs/log_provider.dart';
import 'blocs/user_provider.dart';
import 'blocs/dg_provider.dart';
import 'blocs/work_history_provider.dart';

import 'screens/tab_bar_screen.dart';
import 'screens/login_screen.dart';
import 'screens/overview_grid.dart';

import 'screens/locations_list.dart';

import 'widgets/tab_bar_with_bloc_providers.dart';

import 'widgets/news_notification_widget.dart';

import 'dart:async';

class App extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    /*
    return MaterialApp(
      theme: ThemeData(
        primaryColor: Colors.teal,
      ),
      home: LoginProvider(
        child: StartScreen(),
      ),
      onGenerateRoute: routes,
    ); */

    return LoginProvider(
      child: DgProvider(
        child: WorkHistoryProvider(
          child: DataProvider(
            child: LogProvider(
              child: UserProvider(
                child: TaskProvider(
                  child: CustomerProvider(
                    child: LocationProvider(
                      child: MaterialApp(
                        theme: ThemeData(
                          primaryColor: Colors.teal,
                        ),
                        home: StartScreen(),
                        onGenerateRoute: routes,
                      ),
                    ),
                  ),
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }

  Route<dynamic> routes(RouteSettings settings) {
    if (settings.name.startsWith('data')) {
      var routeParts = settings.name.split('/');
      return MaterialPageRoute(builder: (context) {
        return TabBarWithBlocProviders(
          customerId: int.parse(routeParts[1]),
          userId: int.parse(routeParts[2]),
          locationId: int.parse(routeParts[3]),
          title: routeParts[4],
        );

        return DataProvider(
          customerId: int.parse(routeParts[1]),
          userId: int.parse(routeParts[2]),
          locationId: int.parse(routeParts[3]),
          child: WorkHistoryProvider(
              customerId: int.parse(routeParts[1]),
              userId: int.parse(routeParts[2]),
              locationId: int.parse(routeParts[3]),
              child: DgProvider(
                  customerId: int.parse(routeParts[1]),
                  userId: int.parse(routeParts[2]),
                  locationId: int.parse(routeParts[3]),
                  child: TabBarScreen() /*OverviewGrid()*/)),
        );
      });
    }

    if (settings.name.startsWith('locations')) {
      var routeParts = settings.name.split('/');
      return MaterialPageRoute(builder: (context) {
        var locationScope = routeParts[0] == 'locationsWithoutPlan'
            ? LocationBlocScope.LocationsWithoutPlan
            : routeParts[0] == 'locationsWithoutStaff'
                ? LocationBlocScope.LocationsWithoutStaff
                : routeParts[0] == 'locationsWithoutServiceLeader'
                    ? LocationBlocScope.LocationsWithoutServiceLeader
                    : LocationBlocScope.Locations;

        return LocationProvider(
          scope: locationScope,
          child: DataProvider(
            child: LocationsList(),
            customerId: int.parse(routeParts[1]),
            userId: int.parse(routeParts[2]),
          ),
          customerId: int.parse(routeParts[1]),
          userId: int.parse(routeParts[2]),
        );
      });
    }

    return null;
  }
}

class StartScreen extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    var bloc = LoginProvider.of(context);
    return StreamBuilder(
      stream: bloc.loggedIn,
      builder: (context, AsyncSnapshot<bool> snapshot) {
        if (snapshot.hasData) {
          if (!autoRefreshStarted) {
            autoRefreshStarted = true;
            autoRefresh(context);
          }

          return NewsNotificationWidget(
              child: TabBarScreen(
            title: 'Rent',
          ));
        }
        return LoginScreen();
      },
    );
  }

  bool autoRefreshStarted = false;

  autoRefresh(context) async {
    Future.delayed(Duration(seconds: 30), () {
      var dataBloc = DataProvider.of(context);

      dataBloc.fetchData();
      dataBloc.fetchNews();

      print('Fetching');

      autoRefresh(context);
    });
  }
}
