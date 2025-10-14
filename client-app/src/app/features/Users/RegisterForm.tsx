import { observer } from 'mobx-react-lite'
import { useStore } from '../../stores/store';
import { useState } from 'react';
import { ErrorMessage, Form, Formik } from 'formik';
import * as Yup from 'yup';
import { Button, Dropdown, Header } from 'semantic-ui-react';
import TextInput from '../../common/Form/TextInput';
import DateInput from '../../common/Form/DateInput';
import ValidationError from '../errors/ValidationError';
import { RegisterUserFormValues } from '../../models/User';


export default observer (function RegisterForm() {
    const { userStore } = useStore();
    const [registrationType, setRegistrationType] = useState('ProductionOperator');
  
    const registrationTypeOptions = [
      { key: 'ProductionOperator', text: 'ProductionOperator', value: 'ProductionOperator' },
      { key: 'BusinessUnitLeader', text: 'BusinessUnitLeader', value: 'BusinessUnitLeader' },
      { key: 'QualitySupervisor', text: 'QualitySupervisor', value: 'QualitySupervisor' }
    ];
  
    return (
      <Formik
        initialValues={{
          email: '',
          password: '',
          ime: '',
          prezime: '',
          username: '',
          telefon: '',
          datumRodjenja: new Date(),
          role: registrationType,
          error: null,
        }}
        validationSchema={Yup.object({
          ime: Yup.string().required('Ime je obavezno'),
          prezime: Yup.string().required('Prezime je obavezno'),
          username: Yup.string().required('Korisničko ime je obavezno'),
          email: Yup.string().email('Neispravan email').required('Email je obavezan'),
          telefon: Yup.string().required('Telefon je obavezan'),
          datumRodjenja: Yup.date().required('Datum rođenja je obavezan').nullable(),
        })}

        onSubmit={(values:RegisterUserFormValues, { setErrors }) => {
  // Create an object with the required fields
  const user = {
    email: values.email,
    password: values.password,
    ime: values.ime,
    prezime: values.prezime,
    username: values.username,
    telefon: values.telefon,
    datumRodjenja: values.datumRodjenja, // Make sure datumRodjenja is a Date object
    role: values.role
  };

  // Pass the user object to userStore.register
  userStore
    .register(user) // Pass the plain object
    .catch((error) => setErrors(error.response.data.errors));
}}
      >
        {({ handleSubmit, setFieldValue, isSubmitting, errors, isValid, dirty }) => (
          <Form className="ui form error" onSubmit={handleSubmit} autoComplete="off">
            <Header as="h2" content="Register" color="teal" textAlign="center" />
            <Dropdown
              placeholder="Select User Type"
              fluid
              selection
              options={registrationTypeOptions}
              value={registrationType}
              onChange={(_e, { value }) => {
                setRegistrationType(value as string);
                setFieldValue('role', value as string); // Update the Formik field 'role' with the selected value
              }}
            />
            <TextInput placeholder="Ime" name="ime" />
            <TextInput placeholder="Prezime" name="prezime" />
            <TextInput placeholder="Email" name="email" />
            <TextInput placeholder="Telefon" name="telefon" />
            <TextInput placeholder="Korisnicko ime" name="username" />
            <DateInput 
              placeholderText="Datum rođenja" 
              name="datumRodjenja" 
              showYearDropdown 
              dateFormat="dd/MM/yyyy" 
            />
            
              <TextInput placeholder="Lozinka" name="password" type="password" />
            
            <ErrorMessage
              name="error"
              render={() => <ValidationError errors={errors as unknown as string[]} />}
            />
            <Button
              disabled={!isValid || !dirty || isSubmitting}
              loading={isSubmitting}
              positive
              content="Register"
              type="submit"
              fluid
            />
          </Form>
        )}
      </Formik>
    );
})
