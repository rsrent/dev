import 'package:bms_flutter/src/components/input_dropdown.dart';
import 'package:dart_packages/streamer.dart';

import '../../translations.dart';
import '../components/date_time_picker.dart';
import '../components/streamer_drop_down_button.dart';
import '../components/streamer_text_field.dart';
import 'package:bms_dart/models.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_dart/post_create_update_bloc.dart';
import 'package:rxdart/rxdart.dart';
import 'dart:async';
import '../../style.dart' as style;

class PostCreateUpdateForm extends StatefulWidget {
  @override
  _PostCreateUpdateFormState createState() => _PostCreateUpdateFormState();
}

class _PostCreateUpdateFormState extends State<PostCreateUpdateForm> {
  TextEditingController _titleController;
  TextEditingController _bodyController;

  @override
  Widget build(BuildContext context) {
    var bloc = BlocProvider.of<PostCreateUpdateBloc>(context);

    return BlocListener(
      bloc: bloc,
      listener: (context, PostCreateUpdateState state) {
        print('state $state');

        if (state is PreparingCreate) {
          _titleController = (_titleController ?? TextEditingController());
          _titleController.clear();
          _bodyController = (_bodyController ?? TextEditingController());
          _bodyController.clear();
        }
      },
      child: Padding(
        padding: const EdgeInsets.all(24),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: <Widget>[
            StreamerTextField(
              labelText: 'Titel',
              controller: _titleController,
              streamer: bloc.title,
              maxLines: 1,
            ),
            StreamerTextField(
              labelText: 'Body',
              controller: _bodyController,
              streamer: bloc.body,
              maxLines: 4,
            ),
            StreamBuilder(
              stream: bloc.conditions.stream,
              builder: (BuildContext context,
                  AsyncSnapshot<List<PostCondition>> snapshot) {
                return Container(
                  child: DropdownButtonHideUnderline(
                    child: Column(
                        crossAxisAlignment: CrossAxisAlignment.stretch,
                        children: <Widget>[
                          for (int i = 0; i < (snapshot.data?.length ?? 0); i++)
                            _buildPostConditionTile(
                                context, snapshot.data[i], bloc)
                        ]),
                  ),
                );
              },
            ),
            Divider(color: Colors.transparent),
            FlatButton(
              child: Text(Translations.of(context).buttonAddCondition),
              onPressed: () {
                //bloc.dispatch(AddCondition());
                showAddConditionBottomDrawer(context, bloc);
              },
            ),
            Divider(color: Colors.transparent),
            RaisedButton(
              child: Text(Translations.of(context).buttonCreate),
              onPressed: () {
                bloc.dispatch(CreateRequested());
              },
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildPostConditionTile(BuildContext context, PostCondition condition,
      PostCreateUpdateBloc bloc) {
    var labelString = Translations.of(context).labelPostConditionValue;
    var valueString;
    // Translations.of(context).postConditionCardValueHint;

    var subject = condition.postConditionSubject;
    if (subject is String) {
      valueString = subject;
    }
    if (subject is Location) {
      valueString = subject.displayName;
    }
    if (subject is Customer) {
      valueString = subject.name;
    }

    return Card(
      margin: EdgeInsets.only(top: 16),
      child: Column(
        children: <Widget>[
          Padding(
            padding: const EdgeInsets.only(left: 16),
            child: Row(
              children: <Widget>[
                Expanded(
                  child: Text(
                      '${Translations.of(context).titlePostConditionType} ${Translations.of(context).postConditionTypeString(condition.postConditionType).toLowerCase()}'),
                ),
                IconButton(
                  icon: Icon(Icons.delete),
                  onPressed: () {
                    bloc.dispatch(RemoveCondition(postCondition: condition));
                  },
                ),
              ],
            ),
          ),
          // ChildInputDropdown(
          //   labelText: Translations.of(context).postConditionCardTitle,
          //   isEmpty: condition.postConditionType == null,
          //   child: DropdownButton<PostConditionType>(
          //     value: condition.postConditionType,
          //     onChanged: (v) {
          //       if (condition.postConditionType != v) {
          //         condition.postConditionSubject = null;
          //         condition.postConditionValue = null;
          //       }
          //       condition.postConditionType = v;
          //       bloc.conditions.update(bloc.conditions.value);
          //     },
          //     items: PostConditionType.values
          //         .map<DropdownMenuItem<PostConditionType>>((value) {
          //       return DropdownMenuItem<PostConditionType>(
          //         value: value,
          //         child: Text(
          //           Translations.of(context)
          //               .postConditionTypeString(value),
          //           style: TextStyle(fontSize: 14),
          //         ),
          //       );
          //     }).toList(),
          //   ),
          // ),
          Padding(
            padding: const EdgeInsets.fromLTRB(16, 0, 16, 16),
            child: ValueInputDropdown(
              labelText: labelString,
              valueText: valueString,
              onPressed: () {
                if (condition.postConditionType == PostConditionType.HasRole) {
                  showModalBottomSheet(
                      context: context,
                      builder: (context) {
                        const roles = [
                          'Admin',
                          'Manager',
                          'User',
                          'ClientAdmin',
                          'ClientManager',
                        ];
                        return ListView.builder(
                            itemCount: roles.length,
                            itemBuilder: (context, index) {
                              return ListTile(
                                title: Text(roles[index]),
                                onTap: () {
                                  Navigator.of(context).pop(roles[index]);
                                },
                              );
                            });
                      }).then((v) {
                    if (v != null) {
                      condition.postConditionValue = v;
                      condition.postConditionSubject = v;
                      bloc.conditions.notifyListeners();
                    }
                  });
                }
                if (condition.postConditionType ==
                    PostConditionType.AtLocation) {
                  showModalBottomSheet(
                      context: context,
                      builder: (context) {
                        var locations = bloc.locations;
                        return ListView.builder(
                            itemCount: locations.length,
                            itemBuilder: (context, index) {
                              var location = locations[index];
                              return ListTile(
                                title: Text(
                                    '${location.name}, ${location.customerName}'),
                                onTap: () {
                                  Navigator.of(context).pop(locations[index]);
                                },
                              );
                            });
                      }).then((v) {
                    if (v is Location) {
                      condition.postConditionValue = v.id.toString();
                      condition.postConditionSubject = v;
                      bloc.conditions.notifyListeners();
                    }
                  });
                }
                if (condition.postConditionType ==
                    PostConditionType.AtAnyLocationUnderCustomer) {
                  showModalBottomSheet(
                      context: context,
                      builder: (context) {
                        var customers = bloc.customers;
                        return ListView.builder(
                            itemCount: customers.length,
                            itemBuilder: (context, index) {
                              return ListTile(
                                title: Text(customers[index].name),
                                onTap: () {
                                  Navigator.of(context).pop(customers[index]);
                                },
                              );
                            });
                      }).then((v) {
                    if (v is Customer) {
                      condition.postConditionValue = v.id.toString();
                      condition.postConditionSubject = v;
                      bloc.conditions.notifyListeners();
                    }
                  });
                }
              },
            ),
          ),
        ],
      ),
    );
  }

  void showAddConditionBottomDrawer(
      BuildContext context, PostCreateUpdateBloc bloc) {
    showModalBottomSheet(
        context: context,
        builder: (context) {
          var conditions = PostConditionType.values.toList();
          var existingConditions =
              bloc.conditions.value.map((c) => c.postConditionType).toList();
          conditions.removeWhere((c) => existingConditions.contains(c));
          if (existingConditions
                  .contains(PostConditionType.AtAnyLocationUnderCustomer) ||
              existingConditions.contains(PostConditionType.AtLocation)) {
            conditions.removeWhere((c) =>
                c == PostConditionType.AtAnyLocationUnderCustomer ||
                c == PostConditionType.AtLocation);
          }

          return ListView(
            children: <Widget>[
              ListTile(
                title: Text(
                  '${Translations.of(context).titlePostConditionType}:',
                  style: TextStyle(fontSize: 20),
                ),
              ),
              for (int i = 0; i < conditions.length; i++)
                ListTile(
                  title: Text(Translations.of(context)
                      .postConditionTypeString(conditions[i])),
                  onTap: () {
                    Navigator.of(context).pop(conditions[i]);
                  },
                ),
              ListTile(
                title: Text(
                  Translations.of(context).buttonBack,
                  style: TextStyle(color: style.declineRed),
                ),
                onTap: () {
                  Navigator.of(context).pop();
                },
              )
            ],
          );
        }).then((v) {
      if (v != null) {
        bloc.dispatch(AddCondition(postConditionType: v));
      }
    });
  }
}
