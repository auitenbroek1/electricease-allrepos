import { PageSectionItem, PageSectionItems } from '@/components'

import TutorialModal from '@/components/Tutorial/TutorialModal'

export const TutorialsIndex = () => {
  const section1 = [
    {
      media: `Training_Dashboard.mp4`,
      name: `Dashboard`,
    },
    {
      media: `Training_BidList.mp4`,
      name: `Bid List`,
    },
    {
      media: `Training_JobDetails.mp4`,
      name: `Bid Details`,
    },
    {
      media: `Training_Takeoff_Part1.mp4`,
      name: `Takeoff Part 1`,
    },
    {
      media: `Training_Takeoff_Part2.mp4`,
      name: `Takeoff Part 2`,
    },
    {
      media: `Training_AdditionalExpenses.mp4`,
      name: `Additional Expenses`,
    },
    {
      media: `New+Bid+Summary.mp4`,
      name: `Bid Summary`,
    },
    {
      media: `Training_Adjustments.mp4`,
      name: `Bid Adjustments`,
    },
    {
      media: `Training_BuildProposal.mp4`,
      name: `Build Proposal`,
    },
    {
      media: `Training_SendProposal.mp4`,
      name: `Send Proposal/Work Order`,
    },
    {
      media: `Training_JobSpecificReports.mp4`,
      name: `Reports`,
    },
  ]

  const section2 = [
    {
      media: `Training_Profile.mp4`,
      name: `Profile`,
    },
    // {
    //   media: `Build+Material.mp4`,
    //   name: `Build Material`,
    // },
    {
      media: `Training_BuildAssembly.mp4`,
      name: `Build Assemblies`,
    },
    {
      media: `Training_TagsAndCategories.mp4`,
      name: `Tags and Categories`,
    },
    {
      media: `Training_EditingBidsAddingPhases.mp4`,
      name: `Editing Bids and Adding Phases`,
    },
    {
      media: `Training_AltsAndChangeOrders.mp4`,
      name: `Alternates and Change Orders`,
    },
    {
      media: `Training_LaborFactor.mp4`,
      name: `Labor Factor`,
    },
  ]

  const jumpstart = [
    {
      media: `Jumpstart+Intro+Video+-+Complete.mp4`,
      name: `Welcome to JumpStart`,
    },
    {
      media: `Building+a+Customized+Jumpstart+Bid+Template.mp4`,
      name: `Build a JumpStart Bid Template`,
    },
    {
      media: `Sending+a+Bid+using+Jumpstart.mp4`,
      name: `Sending a Bid with JumpStart`,
    },
    {
      media: `Adding+Multiple+Customers+and+Job+Locations.mp4`,
      name: `JumpStart Use Case Video 1`,
    },
  ]

  return (
    <PageSectionItems>
      <div className="grid grid-cols-2 gap-4">
        <div className="">
          <div className="space-y-4">
            <PageSectionItem
              heading={`Start here to create your first bid`}
              openByDefault={true}
            >
              <div className="space-y-2">
                {section1.map((bid, index) => (
                  <TutorialModal
                    key={index}
                    tutorials={[bid]}
                  />
                ))}
              </div>
            </PageSectionItem>
          </div>
        </div>

        <div className="">
          <div className="space-y-4">
            <PageSectionItem
              heading={`Advanced Tutorials`}
              openByDefault={true}
            >
              <div className="space-y-2">
                {section2.map((item, index) => (
                  <TutorialModal
                    key={index}
                    tutorials={[item]}
                  />
                ))}
              </div>
            </PageSectionItem>
            <PageSectionItem
              heading={`Jumpstart Program`}
              openByDefault={true}
            >
              <div className="space-y-2">
                {jumpstart.map((item: any, index: number) => (
                  <TutorialModal
                    key={index}
                    tutorials={[item]}
                  />
                ))}
              </div>
            </PageSectionItem>
          </div>
        </div>
      </div>
    </PageSectionItems>
  )
}
