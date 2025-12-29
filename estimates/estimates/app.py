import anthropic
import base64
from io import BytesIO
from dotenv import load_dotenv
import os

# Load environment variables from .env file
load_dotenv()

def process_pdf(pdf_path):
    # Get API key from environment variables
    api_key = os.getenv('ANTHROPIC_API_KEY')
    if not api_key:
        raise ValueError("ANTHROPIC_API_KEY not found in .env file")
    
    # Read the PDF file in binary mode
    with open(pdf_path, "rb") as f:
        base64_string = base64.b64encode(f.read()).decode()
    
    # Format the PDF content correctly
    pdf_content = {
        "type": "document",
        "source": {
            "type": "base64",
            "media_type": "application/pdf",
            "data": base64_string
        }
    }
    
    # Initialize the client with PDF beta access
    client = anthropic.Client(
        api_key=api_key,
        default_headers={
            "anthropic-beta": "pdfs-2024-09-25"
        }
    )
    
    # Create the message with correct content structure
    response = client.messages.create(
        model="claude-3-5-sonnet-20241022",
        max_tokens=4096,
        messages=[{
            "role": "user",
            "content": [
                {
                    "type": "text",
                    "text": """
                        You are an electrical estimator. I need you to bid this job and provide a detailed quantity takeoff and estimate.
                        Here is an electrical drawing set in PDF format encoded as base64. I need you to:

                        1. Examine the drawings in detail
                        2. List key project details, including the project name, location, and any other relevant information
                        3. Create a comprehensive electrical estimate including:
                            - Material costs
                            - Labor hours/costs
                            - Equipment requirements
                            - Overhead and profit calculations
                            - Allowances for permits, testing, project management, and as-built documentation
                        4. Be as thorough as possible
                        5. Double check all counts and calculations
                        6. Break down costs by category
                        7. Include detailed quantities for all items
                        8. Include a detailed list of materials required for the project, part names, and manufacturers
                        9. Include a detailed schedule of work
                        10. Include a detailed scope of work
                        11. Include a detailed list of equipment
                        12. Include a detailed list of labor
                        13. Include a detailed list of overhead
                        14. Include a detailed list of profit
                        15. Include a detailed list of contingencies

                        Do not ask any questions.
                    """
                },
                pdf_content
            ]
        }]
    )
    
    return response.content[0].text

# Example usage
try:
    result = process_pdf("plan.pdf")
    print(result)
except Exception as e:
    print(f"Error processing PDF: {str(e)}")