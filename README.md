# NEMESYS

For now, this is going to be a document detailing the project specifications.

NEMESYS is a near-miss exposure and reporting system which allows University students and staff to report dangerous situations that might be present around the UOM campus.

**Types of users: Reporters, Investigators, Administrators**

Reporters can;

* Create a report
* Edit/delete their own reports
* Browse all reports
* Upvote reports

User information (_italic_ = optional);

* Name (string)
* Email (string)
* Password (hashed value/secrets?)
* Number of personal reports (int)
* _Photo (default image)_
* _Phone_

Report properties;

* Date of report (date)
* Location (?)
* Date and time when hazard was spotted (datetime)
* Hazard type (string)
* Description
* Status (enum) - only modifyable by investigators
	* Open
	* No action required
	* Being investigated
	* Closed
* Details of person (user class)
* Number of upvotes
* _Photo_

Investigators can;

* Review reports
* Add an investigation entry to each report before closing the case (only one investigation per report)
* Add and edit investigations for individual reports
* Change the status of a report (_an email is sent to the reporter when status has been changed_)

Investigation properties;

* Description of investigation (string)
* Date of action (date)
* Investigator's details (investigator class)

Optional additional features;

* Integration with Google or Bing maps for report location
* Email notifications when a new report has been submitted (received by investigators - observer pattern?)
* Watching of reports by other reporters (publisher-subscriber pattern?)
* Contact reporter (for investigations)

---

Speculated pages needed (in progress);

* Profile page (for each user)
* Home page (page containing reports, small section of hall of fame with link to actual hall of fame - optional features are sorting by date of report/top upvoted, with tabs for opened, closed, etc reports)
* Hall of fame page based on number of reports (optionally, also based on number of upvotes)
* Login page and register page
* Report page (with ability to edit and delete for the reporter who made the report)
* Investigation page (for investigator to change information)
