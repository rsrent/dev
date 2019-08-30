import 'package:flutter/material.dart';

import '../blocs/customer_provider.dart';
import '../blocs/location_provider.dart';
import '../blocs/data_provider.dart';
import '../blocs/task_provider.dart';
import '../blocs/log_provider.dart';
import '../blocs/user_provider.dart';
import '../blocs/work_history_provider.dart';

import '../screens/tab_bar_screen.dart';

class TabBarWithBlocProviders extends StatelessWidget {
  final int customerId;
  final int userId;
  final int locationId;
  final String title;

  TabBarWithBlocProviders(
      {this.customerId, this.userId, this.locationId, this.title});

  @override
  Widget build(BuildContext context) {
    return WorkHistoryProvider(
      child: DataProvider(
        child: LogProvider(
          child: UserProvider(
            child: TaskProvider(
              child: CustomerProvider(
                child: LocationProvider(
                  child: TabBarScreen(
                    includeCustomers:
                        customerId == 0 && userId == 0 && locationId == 0,
                    includeUsers:
                        customerId == 0 && userId == 0 && locationId == 0,
                    includeLocations: locationId == 0,
                    title: title,
                  ),
                  customerId: customerId,
                  userId: userId,
                ),
              ),
              customerId: customerId,
              userId: userId,
              locationId: locationId,
            ),
          ),
          customerId: customerId,
          userId: userId,
          locationId: locationId,
        ),
        customerId: customerId,
        userId: userId,
        locationId: locationId,
      ),
      customerId: customerId,
      userId: userId,
      locationId: locationId,
    );
  }
}
