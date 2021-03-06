/* tslint:disable */
export interface EmployeeUpdateRequest {
  username: string;
  lastname: string;
  firstname: string;
  email: string;
  phoneNumber: string;
  birthday: string;

  /**
   * The current plain password of the employee, can be null  if a password change shouldn't be performed
   */
  "current-password"?: string;

  /**
   * The new plain password from the employee, can be null if password change shouldn't be performed
   */
  "new-password"?: string;
}
