import 'package:bms_flutter/src/components/animated_bloc_builder.dart';
import 'package:bms_flutter/src/components/project_name.dart';
import 'package:bms_flutter/src/widgets/info_list_view.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/work_contract_list_bloc.dart';
import 'package:flutter/rendering.dart';
import 'package:flutter/widgets.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import '../../../style.dart' as style;

class WorkContractList extends StatelessWidget {
  final Function(WorkContract) onSelect;

  final bool showUser;
  final bool showProject;

  const WorkContractList({
    Key key,
    this.onSelect,
    this.showUser = true,
    this.showProject = true,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final workListBloc = BlocProvider.of<WorkContractListBloc>(context);
    return AnimatedBlocBuilder(
      bloc: workListBloc,
      builder: (context, ListState<WorkContract> state) {
        if (state is Failure) {
          return InfoListView(info: Translations.of(context).infoErrorLoading);
        }

        if (state is Loaded<WorkContract>) {
          if (state.items.isEmpty) {
            return InfoListView(
                info: Translations.of(context).infoNoWorkContracts);
          }

          return ListView.builder(
            padding: EdgeInsets.only(top: 20, bottom: 200),
            itemCount: state.items.length,
            itemBuilder: (BuildContext context, int index) {
              var item = state.items[index];

              return WorkContractTile(
                workContract: item,
                onSelect: onSelect,
                showUser: showUser,
                showProject: showProject,
              );
            },
          );
        }
      },
    );
  }
}

class WorkContractTile extends StatelessWidget {
  final WorkContract workContract;
  final Function(WorkContract) onSelect;
  final bool showUser;
  final bool showProject;

  const WorkContractTile({
    Key key,
    @required this.workContract,
    this.onSelect,
    this.showUser = true,
    this.showProject = true,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    double height = showProject && showUser ? 110 : showUser ? 90 : 80;

    Color color = Theme.of(context).primaryColor;
    if (!workContract.isOwned) color = style.declineRed;

    return Padding(
      padding: const EdgeInsets.only(bottom: 8.0),
      child: InkWell(
        child: Container(
          height: height,
          color: color,
          //padding: const EdgeInsets.only(right: 6),
          child: Row(
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: <Widget>[
              Expanded(
                child: Container(
                  padding: EdgeInsets.only(right: 16, top: 8, bottom: 8),
                  decoration: BoxDecoration(
                    color: Color(0xFFF1F9FF),
                  ),
                  child: Row(
                    children: <Widget>[
                      Expanded(
                        child: (showProject && !showUser)
                            ? ListTile(
                                title:
                                    ProjectName(project: workContract.project),
                              )
                            : Column(
                                crossAxisAlignment: CrossAxisAlignment.stretch,
                                mainAxisAlignment: MainAxisAlignment.center,
                                children: <Widget>[
                                  if (showProject)
                                    Padding(
                                      padding: EdgeInsets.only(left: 16),
                                      child: ProjectName(
                                          project: workContract.project),
                                    ),
                                  ListTile(
                                    contentPadding: EdgeInsets.only(left: 16),
                                    leading: workContract.isOwned
                                        ? CircleAvatar()
                                        : null,
                                    title: Text(
                                      !workContract.isOwned
                                          ? Translations.of(context)
                                              .infoWorkContractMissingOwner
                                          : workContract.user.displayName,
                                      maxLines: 2,
                                      overflow: TextOverflow.ellipsis,
                                      style: TextStyle(
                                          color: !workContract.isOwned
                                              ? style.declineRed
                                              : null),
                                    ),
                                  )
                                ],
                              ),
                      ),
                      Container(
                        child: Column(
                          mainAxisAlignment: MainAxisAlignment.center,
                          children: <Widget>[
                            Text(Translations.of(context)
                                .dateString(workContract.fromDate)),
                            Text(Translations.of(context)
                                .dateString(workContract.toDate)),
                          ],
                        ),
                      ),
                    ],
                  ),
                ),
              ),
              SizedBox(width: 6)
            ],
          ),
        ),
        onTap: () => onSelect(workContract),
      ),
    );
  }
}
