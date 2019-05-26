/* tslint:disable */
export interface RoleAddToEmployeeRequest {

  /**
   * Id of the role which should be added to the employee
   */
  "role-id": string;

  /**
   * Id of the employee which the role should be added to
   */
  "employee-id": string;
}