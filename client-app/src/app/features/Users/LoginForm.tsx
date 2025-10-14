import { ErrorMessage, Formik } from "formik"
import { observer } from "mobx-react-lite"
import userStore from "../../stores/userStore"
import { Button, Form, Header, Label } from "semantic-ui-react"
import TextInput from "../../common/Form/TextInput"
import { useStore } from "../../stores/store"

export default observer( function LoginForm() {
  const {userStore, modalStore} = useStore();

  return (
    <Formik 
        initialValues={{email:'', password:'', error:null}}
        onSubmit={(values, {setErrors})=>userStore.login(values).catch((error)=> 
          setErrors({error: error.response.data}))}
    >
        {({handleSubmit, isSubmitting, errors})=>(
            <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                <Header as='h2' content='Prijavi se' color='teal' textAlign='center' />
                <TextInput placeholder='Email' name='email' />
                <TextInput placeholder='Lozinka' name='password' type='password' />
                <ErrorMessage 
                  name='error' render={()=><Label style={{marginBottom:10}} basic color='red' content={errors.error} />} />
                
                <Button loading={isSubmitting} positive content='Prijavi se' type='submit' fluid />
            </Form>
        )}
    </Formik>
  )
})
