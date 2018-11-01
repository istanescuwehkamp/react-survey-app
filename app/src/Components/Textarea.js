import React, { Component } from 'react'

export class Textarea extends Component {
    constructor(props) {
        super(props);
        this.state = {
            inputAnswer: ''
        }
        this.handleChange = this.handleChange.bind(this);
    }
    handleChange(event) {
        this.setState({inputAnswer: event.target.value}, () => {this.props.saveAnswers(this.state.inputAnswer)})
    }
  render() {
    return (
        <div className="question-options">
            <div className="input-group">
                <textarea className="form-textarea" 
                          value={this.state.inputAnswer} 
                          placeholder='Write your response here...'
                          onChange={this.handleChange}
                          aria-label="With textarea">
                </textarea>
            </div>    
        </div>
    )
  }
}

export default Textarea;