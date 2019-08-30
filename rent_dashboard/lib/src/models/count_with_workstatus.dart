import 'workstatus.dart';

class CountWithWorkStatus {
  final WorkStatus status;
  int _occurences;
  bool include = true;
  int get occurences => include ? _occurences : 0;
  CountWithWorkStatus(this.status, this._occurences);
}

class CountWithWorkStatusSet {
  CountWithWorkStatus delayed;
  CountWithWorkStatus critical;
  CountWithWorkStatus okay;
  CountWithWorkStatus unstarted;
  CountWithWorkStatusSet({json, List<int> nums}) {
    if (json != null) {
      delayed = CountWithWorkStatus(WorkStatus.Overdue, json['delayed']);
      critical = CountWithWorkStatus(WorkStatus.Critical, json['critical']);
      okay = CountWithWorkStatus(WorkStatus.Okay, json['okay']);
      unstarted = CountWithWorkStatus(WorkStatus.Unstarted, json['unstarted']);
    }
    if (nums != null) {
      delayed = CountWithWorkStatus(WorkStatus.Overdue, nums[0]);
      critical = CountWithWorkStatus(WorkStatus.Critical, nums[1]);
      okay = CountWithWorkStatus(WorkStatus.Okay, nums[2]);
      unstarted = CountWithWorkStatus(WorkStatus.Unstarted, nums[3]);
    }
  }

  List<CountWithWorkStatus> asList() => [delayed, critical, okay, unstarted];

  getAmount(WorkStatus status) {
    switch (status) {
      case WorkStatus.Overdue:
        return delayed;
      case WorkStatus.Critical:
        return critical;
      case WorkStatus.Okay:
        return okay;
      case WorkStatus.Unstarted:
        return unstarted;
      case WorkStatus.Ignored:
        return 0;
    }
  }

  int comparedTo(CountWithWorkStatusSet w2)
  {
    if(delayed.occurences != w2.delayed.occurences)
      return w2.delayed.occurences.compareTo(delayed.occurences);
    if(critical.occurences != w2.critical.occurences)
      return w2.critical.occurences.compareTo(critical.occurences);
    if(okay.occurences != w2.okay.occurences)
      return w2.okay.occurences.compareTo(okay.occurences);
    return w2.unstarted.occurences.compareTo(unstarted.occurences);
  }
}